using System.Text.Json;
using System.Text.Json.Serialization;
using Confluent.Kafka;
using SPbSTU.OPD.ASAP.Google.Application.Interfaces;
using SPbSTU.OPD.ASAP.Google.Application.Services;
using SPbSTU.OPD.ASAP.Google.Domain.Interfaces;
using SPbSTU.OPD.ASAP.Google.Infrastructure.Common;
using SPbSTU.OPD.ASAP.Google.Infrastructure.Contracts;
using SPbSTU.OPD.ASAP.Google.Infrastructure.GoogleSheets;
using SPbSTU.OPD.ASAP.Google.Infrastructure.Kafka;
using SPbSTU.OPD.ASAP.Google.Infrastructure.Settings;
using SPbSTU.OPD.ASAP.Google.Kafka;
using SPbSTU.OPD.ASAP.Google.Services;
using Microsoft.Extensions.Options;

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
            services.AddScoped<ISpreadSheetBuilder, SpreadSheetBuilder>();
            services.AddScoped<IGoogleClientFactory, GoogleClientFactory>();
            services.Configure<GoogleOptions>(
                configuration.GetSection(GoogleOptions.SectionName));
            services.AddScoped<ISheetUpdater, SheetUpdater>();
            services.AddScoped<IUpdateSheetService, UpdateSheetService>();
        //
        services.AddScoped<IHandler<Ignore, PointsGoogleKafka>, PointsHandler>();
        services.AddKafkaHandler<Ignore, PointsGoogleKafka>(
            KafkaOptions.Points,
            null,
            new SystemTextJsonSerializer<PointsGoogleKafka>());

        services.AddScoped<IHandler<Ignore, QueueKafka>, QueueHandler>();
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