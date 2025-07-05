using Confluent.Kafka;

namespace SPbSTU.OPD.ASAP.Google.Infrastructure.Kafka;

public interface IHandler<TKey, TValue>
{
    Task Handle(IReadOnlyCollection<ConsumeResult<TKey, TValue>> messages, CancellationToken token);
}