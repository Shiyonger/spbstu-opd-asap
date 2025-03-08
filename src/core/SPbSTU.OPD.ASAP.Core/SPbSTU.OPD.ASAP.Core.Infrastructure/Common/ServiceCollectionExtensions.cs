using System.Reflection;
using Confluent.Kafka;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Processors;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SPbSTU.OPD.ASAP.Core.Infrastructure.Kafka;
using SPbSTU.OPD.ASAP.Core.Infrastructure.Settings;

namespace SPbSTU.OPD.ASAP.Core.Infrastructure.Common;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFluentMigrator(
        this IServiceCollection services,
        string connectionString,
        Assembly assembly)
    {
        services
            .AddFluentMigratorCore()
            .ConfigureRunner(
                builder => builder
                    .AddPostgres()
                    .ScanIn(assembly).For.Migrations())
            .AddOptions<ProcessorOptions>()
            .Configure(
                options =>
                {
                    options.ProviderSwitches = "Force Quote=false";
                    options.Timeout = TimeSpan.FromMinutes(10);
                    options.ConnectionString = connectionString;
                });

        return services;
    }
    
    public static IServiceCollection AddKafkaHandler<TKey, TValue, THandler>(
        this IServiceCollection services,
        IConfiguration configuration,
        IDeserializer<TKey>? keyDeserializer,
        IDeserializer<TValue>? valueDeserializer) where THandler : IHandler<TKey, TValue>
    {
        services.Configure<KafkaOptions>(configuration.GetSection(nameof(KafkaOptions)));

        services.AddSingleton<KafkaAsyncConsumer<TKey, TValue>>(
            provider =>
            {
                var handler = provider.GetRequiredService<THandler>();
                var options = provider.GetRequiredService<IOptions<KafkaOptions>>();
                var logger = provider.GetRequiredService<ILogger<KafkaAsyncConsumer<TKey, TValue>>>();

                return new KafkaAsyncConsumer<TKey, TValue>(
                    handler,
                    options.Value,
                    keyDeserializer,
                    valueDeserializer,
                    logger);
            });

        return services;
    }
}