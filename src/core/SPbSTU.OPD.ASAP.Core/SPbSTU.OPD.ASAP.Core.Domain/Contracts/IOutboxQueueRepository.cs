using SPbSTU.OPD.ASAP.Core.Domain.Models;

namespace SPbSTU.OPD.ASAP.Core.Domain.Contracts;

public interface IOutboxQueueRepository
{
    Task<List<long>> Create(List<OutboxQueueCreateModel> queueQuery, CancellationToken token);
    
    Task<List<OutboxQueueGetModel>> GetNotSent(CancellationToken token);
    
    Task UpdateSent(List<long> queueQueryIds, CancellationToken token);
}