using Confluent.Kafka;

namespace SPbSTU.OPD.ASAP.Core.Infrastructure.Kafka;

public interface IHandler<TKey, TValue>
{
    Task Handle(IReadOnlyCollection<ConsumeResult<TKey, TValue>> messages, CancellationToken token);
}