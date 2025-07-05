using Confluent.Kafka;
using SPbSTU.OPD.ASAP.Core.Infrastructure.Contracts;
using SPbSTU.OPD.ASAP.Core.Infrastructure.Kafka;

namespace SPbSTU.OPD.ASAP.Core.Kafka;

public class KafkaBackgroundService(
    KafkaAsyncConsumer<Ignore, PointsGithubKafka> pointsConsumer,
    KafkaAsyncConsumer<Ignore, ActionKafka> actionConsumer,
    ILogger<KafkaBackgroundService> logger)
    : BackgroundService
{
    public override Task StopAsync(CancellationToken cancellationToken)
    {
        pointsConsumer.Dispose();

        return Task.CompletedTask;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            var queue = actionConsumer.Consume(stoppingToken);
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