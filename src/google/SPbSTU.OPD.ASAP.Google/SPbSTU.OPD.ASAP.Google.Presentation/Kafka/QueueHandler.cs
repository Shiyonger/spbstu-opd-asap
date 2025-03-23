using Confluent.Kafka;
using SPbSTU.OPD.ASAP.Google.Infrastructure.Kafka;

namespace SPbSTU.OPD.ASAP.Google.Kafka;

public class QueueHandler(ILogger<QueueHandler> logger) : IHandler<Ignore, string>
{
    // TODO: write proper handler
    public Task Handle(IReadOnlyCollection<ConsumeResult<Ignore, string>> messages, CancellationToken token)
    {
        Task.Delay(100, token);
        logger.LogInformation("Handled {Count} messages", messages.Count);
        return Task.CompletedTask;
    }
}