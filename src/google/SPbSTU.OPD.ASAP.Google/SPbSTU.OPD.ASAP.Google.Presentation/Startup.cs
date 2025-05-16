using System.Text.Json;
using System.Text.Json.Serialization;
using Confluent.Kafka;
using SPbSTU.OPD.ASAP.Google.Application.Interfaces;
using SPbSTU.OPD.ASAP.Google.Application.Services;
using SPbSTU.OPD.ASAP.Google.Domain.Interfaces;
using SPbSTU.OPD.ASAP.Google.Infrastructure.Common;
using SPbSTU.OPD.ASAP.Google.Infrastructure.Contracts;
using SPbSTU.OPD.ASAP.Google.Infrastructure.Kafka;
using SPbSTU.OPD.ASAP.Google.Infrastructure.Settings;
using SPbSTU.OPD.ASAP.Google.Kafka;
using SPbSTU.OPD.ASAP.Google.Services;

namespace SPbSTU.OPD.ASAP.Google;

public sealed class Startup(IConfiguration configuration)
{
    public void ConfigureServices(IServiceCollection services)
    {
        services
            .AddLogging();
        services.AddGrpc();

        services.Configure<KafkaOptions>(KafkaOptions.Points,
            configuration.GetSection("KafkaOptions:Points"));
        services.Configure<KafkaOptions>(KafkaOptions.Queue,
            configuration.GetSection("KafkaOptions:Queue"));
        
        //--------------------------------------------------------------
        
        services.AddScoped<ISpreadSheetService, SpreadSheetService>();
        
        services.Configure<GoogleOptions>(
            configuration.GetSection(GoogleOptions.SectionName));
        
        //
        services.AddScoped<PointsHandler>();
        services.AddKafkaHandler<Ignore, PointsKafka>(
            KafkaOptions.Points,
            null,
            new SystemTextJsonSerializer<PointsKafka>());

        services.AddScoped<QueueHandler>();
        services.AddKafkaHandler<Ignore, QueueKafka>(
            KafkaOptions.Queue,
            null,
            new SystemTextJsonSerializer<QueueKafka>(new JsonSerializerOptions
                { Converters = { new JsonStringEnumConverter() } }));

        // services.AddHostedService<KafkaBackgroundService>();

        services.AddGrpcReflection();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapGrpcService<SpreadSheetsGrpcService>();
            endpoints.MapGrpcReflectionService();
        });
    }
}