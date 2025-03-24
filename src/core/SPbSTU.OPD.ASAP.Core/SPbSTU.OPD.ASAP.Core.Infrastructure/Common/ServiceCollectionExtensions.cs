using System.Reflection;
using Confluent.Kafka;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Processors;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Npgsql;
using Npgsql.NameTranslation;
using SPbSTU.OPD.ASAP.Core.Infrastructure.Entities;
using SPbSTU.OPD.ASAP.Core.Infrastructure.Kafka;
using SPbSTU.OPD.ASAP.Core.Infrastructure.Settings;

namespace SPbSTU.OPD.ASAP.Core.Infrastructure.Common;

public static class ServiceCollectionExtensions
{
    private static readonly INpgsqlNameTranslator Translator = new NpgsqlSnakeCaseNameTranslator();
    
    public static void MapCompositeTypes(this IServiceCollection services)
    {
        var mapper = NpgsqlConnection.GlobalTypeMapper;
        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

        mapper.MapComposite<OutboxPointsEntityV1>("outbox_points_v1", Translator);
        mapper.MapComposite<OutboxQueueEntityV1>("outbox_queue_v1", Translator);
    }
    
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
        IDeserializer<TKey>? keyDeserializer,
        IDeserializer<TValue>? valueDeserializer) where THandler : IHandler<TKey, TValue>
    {
        services.AddSingleton<KafkaAsyncConsumer<TKey, TValue>>(
            provider =>
            {
                var scopeFactory = provider.GetRequiredService<IServiceScopeFactory>();
                var options = provider.GetRequiredService<IOptions<KafkaConsumerOptions>>();
                var logger = provider.GetRequiredService<ILogger<KafkaAsyncConsumer<TKey, TValue>>>();

                return new KafkaAsyncConsumer<TKey, TValue>(
                    scopeFactory,
                    options.Value,
                    keyDeserializer,
                    valueDeserializer,
                    logger);
            });

        return services;
    }
    
    public static IServiceCollection AddKafkaPublisher<TKey, TValue>(
        this IServiceCollection services,
        string topic,
        ISerializer<TKey>? keyDeserializer,
        ISerializer<TValue>? valueDeserializer)
    {
        services.AddSingleton<KafkaPublisher<TKey, TValue>>(
            provider =>
            {
                var options = provider.GetRequiredService<IOptionsMonitor<KafkaPublisherOptions>>().Get(topic);

                return new KafkaPublisher<TKey, TValue>(
                    options,
                    keyDeserializer,
                    valueDeserializer);
            });

        return services;
    }
}