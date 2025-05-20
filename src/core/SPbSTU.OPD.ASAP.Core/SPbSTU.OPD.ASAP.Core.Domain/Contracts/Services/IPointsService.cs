using SPbSTU.OPD.ASAP.Core.Domain.Models.Outbox.Points;

namespace SPbSTU.OPD.ASAP.Core.Domain.Contracts.Services;

public interface IPointsService
{
    Task CreatePointsAndQueue(List<PointsGithub> pointsGithub, CancellationToken token);
    
    Task<List<OutboxPointsGetModel>> GetNotSentPoints(CancellationToken token);
    
    Task UpdateSentPoints(List<long> sentPointsIds, CancellationToken token);
}