using SPbSTU.OPD.ASAP.Core.Domain.Models;

namespace SPbSTU.OPD.ASAP.Core.Domain.Contracts;

public interface IOutboxQueueRepository
{
    Task<List<long>> Create(List<OutboxQueue> queueQuery, CancellationToken token);
    
    Task<List<OutboxQueue>> GetNotSent(CancellationToken token);
    
    Task UpdateSent(List<long> queueQueryIds, CancellationToken token);
}