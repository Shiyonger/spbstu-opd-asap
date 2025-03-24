using Confluent.Kafka;
using SPbSTU.OPD.ASAP.Core.Infrastructure.Kafka;

namespace SPbSTU.OPD.ASAP.Core.Kafka;

public class KafkaBackgroundService(KafkaAsyncConsumer<Ignore, string> kafkaConsumer, ILogger<KafkaBackgroundService> logger)
    : BackgroundService
{
    public override Task StopAsync(CancellationToken cancellationToken)
    {
        kafkaConsumer.Dispose();

        return Task.CompletedTask;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            await kafkaConsumer.Consume(stoppingToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception occured");
        }
    }
}