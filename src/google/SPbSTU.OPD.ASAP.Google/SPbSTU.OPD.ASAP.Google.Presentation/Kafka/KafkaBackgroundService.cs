using Confluent.Kafka;
using SPbSTU.OPD.ASAP.Google.Infrastructure.Contracts;
using SPbSTU.OPD.ASAP.Google.Infrastructure.Kafka;

namespace SPbSTU.OPD.ASAP.Google.Kafka;

public class KafkaBackgroundService(
    KafkaAsyncConsumer<Ignore, PointsGoogleKafka> pointsConsumer,
    KafkaAsyncConsumer<Ignore, QueueKafka> queueConsumer,
    ILogger<KafkaBackgroundService> logger)
    : BackgroundService
{
    public override Task StopAsync(CancellationToken cancellationToken)
    {
        pointsConsumer.Dispose();
        queueConsumer.Dispose();

        return Task.CompletedTask;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            var queue = queueConsumer.Consume(stoppingToken);
            await Task.Delay(2000, stoppingToken).WaitAsync(stoppingToken);
            var points = pointsConsumer.Consume(stoppingToken);
            await Task.WhenAll(points, queue);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception occured");
        }
    }
}