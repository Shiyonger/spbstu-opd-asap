using System.Text.Json;
using System.Text.Json.Serialization;
using Confluent.Kafka;
using SPbSTU.OPD.ASAP.Core.Application.Services;
using SPbSTU.OPD.ASAP.Core.Domain.Contracts;
using SPbSTU.OPD.ASAP.Core.Domain.Models;
using SPbSTU.OPD.ASAP.Core.Infrastructure.Common;
using SPbSTU.OPD.ASAP.Core.Infrastructure.Contracts;
using SPbSTU.OPD.ASAP.Core.Infrastructure.Kafka;
using SPbSTU.OPD.ASAP.Core.Infrastructure.Repositories;
using SPbSTU.OPD.ASAP.Core.Infrastructure.Settings;
using SPbSTU.OPD.ASAP.Core.Kafka;

namespace SPbSTU.OPD.ASAP.Core;

public sealed class Startup(IConfiguration configuration)
{
    public void ConfigureServices(IServiceCollection services)
    {
        services
            .AddLogging();
        services.AddGrpc();

        var connectionString = configuration["PostgresConnectionString"]!;

        services.MapCompositeTypes();
        services
            .AddFluentMigrator(
                connectionString,
                typeof(SqlMigration).Assembly);

        services.Configure<KafkaConsumerOptions>(configuration.GetSection(nameof(KafkaConsumerOptions)));
        services.Configure<KafkaPublisherOptions>(KafkaPublisherOptions.Points,
            configuration.GetSection("KafkaPublisherOptions:Points"));
        services.Configure<KafkaPublisherOptions>(KafkaPublisherOptions.Queue,
            configuration.GetSection("KafkaPublisherOptions:Queue"));

        services.AddScoped<ItemHandler>();
        services.AddKafkaHandler<Ignore, string, ItemHandler>(
            null,
            null);
        services.AddHostedService<KafkaBackgroundService>();

        services.AddScoped<IOutboxPointsRepository, OutboxPointsRepository>(_ =>
            new OutboxPointsRepository(connectionString));
        services.AddScoped<IOutboxQueueRepository, OutboxQueueRepository>(_ =>
            new OutboxQueueRepository(connectionString));
        services.AddScoped<IOutboxService, OutboxService>();

        services.AddKafkaPublisher<long, PointsKafka>(
            KafkaPublisherOptions.Points,
            null,
            new SystemTextJsonSerializer<PointsKafka>(new JsonSerializerOptions
                { Converters = { new JsonStringEnumConverter() } }));
        services.AddKafkaPublisher<long, QueueKafka>(
            KafkaPublisherOptions.Queue,
            null,
            new SystemTextJsonSerializer<QueueKafka>(new JsonSerializerOptions
                { Converters = { new JsonStringEnumConverter() } }));
        services.AddHostedService<OutboxBackgroundService>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapGet("/",
                () =>
                    "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
        });
    }
}