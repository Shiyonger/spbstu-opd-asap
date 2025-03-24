using SPbSTU.OPD.ASAP.Core.Domain.Models;

namespace SPbSTU.OPD.ASAP.Core.Domain.Contracts;

public interface IOutboxPointsRepository
{
    Task<List<long>> Create(List<OutboxPoints> points, CancellationToken token);
    
    Task<List<OutboxPoints>> GetNotSent(CancellationToken token);
    
    Task UpdateSent(List<long> sentPointsIds, CancellationToken token);
}