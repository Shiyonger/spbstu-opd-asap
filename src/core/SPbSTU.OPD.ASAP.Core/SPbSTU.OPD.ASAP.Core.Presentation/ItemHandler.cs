using Confluent.Kafka;
using SPbSTU.OPD.ASAP.Core.Infrastructure.Kafka;

namespace SPbSTU.OPD.ASAP.Core;

public class ItemHandler(ILogger<ItemHandler> logger) : IHandler<Ignore, string>
{
    public Task Handle(IReadOnlyCollection<ConsumeResult<Ignore, string>> messages, CancellationToken token)
    {
        Task.Delay(100, token);
        logger.LogInformation("Handled {Count} messages", messages.Count);
        return Task.CompletedTask;
    }
}
