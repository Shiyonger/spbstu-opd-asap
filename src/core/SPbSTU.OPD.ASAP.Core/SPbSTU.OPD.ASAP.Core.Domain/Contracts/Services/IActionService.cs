using SPbSTU.OPD.ASAP.Core.Domain.Models.Outbox.Queue;

namespace SPbSTU.OPD.ASAP.Core.Domain.Contracts.Services;

public interface IActionService
{
    Task CreateQueueAndSubmissions(List<ActionGithub> actionGithubs, CancellationToken token);
    
    Task<List<OutboxQueueGetModel>> GetNotSentQueue(CancellationToken token);
    
    Task UpdateSentQueue(List<long> queueQueryIds, CancellationToken token);
}