using System.Text.Json;
using System.Text.Json.Serialization;
using Confluent.Kafka;
using SPbSTU.OPD.ASAP.Core.Application.Extensions;
using SPbSTU.OPD.ASAP.Core.Application.Services;
using SPbSTU.OPD.ASAP.Core.Domain.Contracts;
using SPbSTU.OPD.ASAP.Core.Domain.Contracts.Repositories;
using SPbSTU.OPD.ASAP.Core.Domain.Contracts.Services;
using SPbSTU.OPD.ASAP.Core.Infrastructure.Common;
using SPbSTU.OPD.ASAP.Core.Infrastructure.Contracts;
using SPbSTU.OPD.ASAP.Core.Infrastructure.Kafka;
using SPbSTU.OPD.ASAP.Core.Infrastructure.Settings;
using SPbSTU.OPD.ASAP.Core.Kafka;
using SPbSTU.OPD.ASAP.Core.Kafka.Handlers;
using SPbSTU.OPD.ASAP.Core.Persistence.Common;
using SPbSTU.OPD.ASAP.Core.Persistence.Repositories;
using SPbSTU.OPD.ASAP.Core.Presentation;
using SPbSTU.OPD.ASAP.Core.Services;
using AssignmentsService = SPbSTU.OPD.ASAP.Core.Application.Services.AssignmentsService;
using CoursesService = SPbSTU.OPD.ASAP.Core.Application.Services.CoursesService;

namespace SPbSTU.OPD.ASAP.Core;

public sealed class Startup(IConfiguration configuration)
{
    public void ConfigureServices(IServiceCollection services)
    {
        services
            .AddLogging()
            .AddGrpc();

        services.AddGrpcClient<SpreadSheetsService.SpreadSheetsServiceClient>(o =>
            o.Address = new Uri(configuration["GrpcGoogleUri"]!));

        var connectionString = configuration["PostgresConnectionString"]!;

        services.MapCompositeTypes();
        services.AddFluentMigrator(
            connectionString,
            typeof(SqlMigration).Assembly);

        services
            .Configure<KafkaPublisherOptions>(KafkaPublisherOptions.Points,
                configuration.GetSection("KafkaPublisherOptions:Points"))
            .Configure<KafkaPublisherOptions>(KafkaPublisherOptions.Queue,
                configuration.GetSection("KafkaPublisherOptions:Queue"))
            .Configure<KafkaConsumerOptions>(KafkaConsumerOptions.Points,
                configuration.GetSection("KafkaConsumerOptions:Points"))
            .Configure<KafkaConsumerOptions>(KafkaConsumerOptions.Action,
                configuration.GetSection("KafkaConsumerOptions:Action"));

        services
            .AddPersistence(connectionString)
            .AddApplication();

        services
            .AddScoped<IHandler<Ignore, ActionKafka>, ActionHandler>()
            .AddScoped<IHandler<Ignore, PointsGithubKafka>, PointsGithubHandler>();

        services
            .AddKafkaHandler<Ignore, ActionKafka>(
                KafkaConsumerOptions.Action,
                null,
                new SystemTextJsonSerializer<ActionKafka>(new JsonSerializerOptions
                    { Converters = { new JsonStringEnumConverter() } }))
            .AddKafkaHandler<Ignore, PointsGithubKafka>(
                KafkaConsumerOptions.Points,
                null,
                new SystemTextJsonSerializer<PointsGithubKafka>());

        services
            .AddKafkaPublisher<long, PointsGoogleKafka>(
                KafkaPublisherOptions.Points,
                null,
                new SystemTextJsonSerializer<PointsGoogleKafka>())
            .AddKafkaPublisher<long, QueueKafka>(
                KafkaPublisherOptions.Queue,
                null,
                new SystemTextJsonSerializer<QueueKafka>(new JsonSerializerOptions
                    { Converters = { new JsonStringEnumConverter() } }));

        services
            .AddHostedService<KafkaBackgroundService>()
            .AddHostedService<OutboxBackgroundService>()
            .AddHostedService<SpreadSheetsBackgroundService>();

        services.AddGrpcReflection();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapGrpcService<UsersGrpcService>();
            endpoints.MapGrpcService<AssignmentsGrpcService>();
            endpoints.MapGrpcService<CoursesGrpcService>();
            endpoints.MapGrpcService<GithubGrpcService>();
            endpoints.MapGrpcReflectionService();
        });
    }
}