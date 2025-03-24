using Confluent.Kafka;
using SPbSTU.OPD.ASAP.Core.Infrastructure.Common;
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

        services.AddScoped<ItemHandler>();
        services.AddKafkaHandler<Ignore, string, ItemHandler>(
            configuration,
            null,
            null);
        services.AddHostedService<KafkaBackgroundService>();
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