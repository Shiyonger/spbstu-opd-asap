using SPbSTU.OPD.ASAP.Core.Domain.Contracts;
using SPbSTU.OPD.ASAP.Core.Domain.Contracts.Repositories;
using SPbSTU.OPD.ASAP.Core.Domain.Contracts.Services;
using SPbSTU.OPD.ASAP.Core.Domain.Models;

namespace SPbSTU.OPD.ASAP.Core.Application.Services;

// Может потом заменить, сделать частью другого сервиса
public class OutboxService(IOutboxPointsRepository pointsRepository, IOutboxQueueRepository queueRepository)
    : IOutboxService
{
    private readonly IOutboxPointsRepository _pointsRepository = pointsRepository;
    private readonly IOutboxQueueRepository _queueRepository = queueRepository;

    public Task<List<long>> CreatePoints(List<OutboxPointsCreateModel> points, CancellationToken token)
    {
        return _pointsRepository.Create(points, token);
    }

    public Task<List<OutboxPointsGetModel>> GetNotSentPoints(CancellationToken token)
    {
        return _pointsRepository.GetNotSent(token);
    }

    public Task UpdateSentPoints(List<long> sentPointsIds, CancellationToken token)
    {
        return _pointsRepository.UpdateSent(sentPointsIds, token);
    }

    public Task<List<long>> CreateQueue(List<OutboxQueueCreateModel> queueQuery, CancellationToken token)
    {
        return _queueRepository.Create(queueQuery, token);
    }

    public Task<List<OutboxQueueGetModel>> GetNotSentQueue(CancellationToken token)
    {
        return _queueRepository.GetNotSent(token);
    }

    public Task UpdateSentQueue(List<long> queueQueryIds, CancellationToken token)
    {
        return _queueRepository.UpdateSent(queueQueryIds, token);
    }
}