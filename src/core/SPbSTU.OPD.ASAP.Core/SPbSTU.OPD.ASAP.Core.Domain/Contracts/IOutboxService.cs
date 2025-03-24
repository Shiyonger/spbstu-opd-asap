using SPbSTU.OPD.ASAP.Core.Domain.Models;

namespace SPbSTU.OPD.ASAP.Core.Domain.Contracts;

public interface IOutboxService
{
    Task<List<long>> CreatePoints(List<OutboxPointsCreateModel> points, CancellationToken token);
    
    Task<List<OutboxPointsGetModel>> GetNotSentPoints(CancellationToken token);
    
    Task UpdateSentPoints(List<long> sentPointsIds, CancellationToken token);
    
    Task<List<long>> CreateQueue(List<OutboxQueueCreateModel> queueQuery, CancellationToken token);
    
    Task<List<OutboxQueueGetModel>> GetNotSentQueue(CancellationToken token);
    
    Task UpdateSentQueue(List<long> queueQueryIds, CancellationToken token);
}