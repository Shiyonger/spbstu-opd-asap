using SPbSTU.OPD.ASAP.Core.Domain.Models;
using SPbSTU.OPD.ASAP.Core.Domain.Models.Outbox;
using SPbSTU.OPD.ASAP.Core.Domain.Models.Outbox.Points;

namespace SPbSTU.OPD.ASAP.Core.Domain.Contracts.Repositories;

public interface IOutboxPointsRepository
{
    Task<List<long>> Create(List<OutboxPointsCreateModel> points, CancellationToken token);
    
    Task<List<OutboxPointsGetModel>> GetNotSent(CancellationToken token);
    
    Task UpdateSent(List<long> sentPointsIds, CancellationToken token);
}