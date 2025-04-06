using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SPbSTU.OPD.ASAP.Core.Infrastructure.Kafka;
using SPbSTU.OPD.ASAP.Core.Infrastructure.Settings;

namespace SPbSTU.OPD.ASAP.Core.Infrastructure.Common;

public static class ServiceCollectionExtensions
{
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