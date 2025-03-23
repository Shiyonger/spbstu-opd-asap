using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SPbSTU.OPD.ASAP.Google.Infrastructure.Kafka;
using SPbSTU.OPD.ASAP.Google.Infrastructure.Settings;

namespace SPbSTU.OPD.ASAP.Google.Infrastructure.Common;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddKafkaHandler<TKey, TValue>(
        this IServiceCollection services,
        string topic,
        IDeserializer<TKey>? keyDeserializer,
        IDeserializer<TValue>? valueDeserializer)
    {
        services.AddSingleton<KafkaAsyncConsumer<TKey, TValue>>(
            provider =>
            {
                var scopeFactory = provider.GetRequiredService<IServiceScopeFactory>();
                var options = provider.GetRequiredService<IOptionsMonitor<KafkaOptions>>().Get(topic);
                var logger = provider.GetRequiredService<ILogger<KafkaAsyncConsumer<TKey, TValue>>>();

                return new KafkaAsyncConsumer<TKey, TValue>(
                    scopeFactory,
                    options,
                    keyDeserializer,
                    valueDeserializer,
                    logger);
            });

        return services;
    }
}