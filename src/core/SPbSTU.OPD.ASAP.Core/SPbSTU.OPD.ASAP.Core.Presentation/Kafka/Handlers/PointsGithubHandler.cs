using Confluent.Kafka;
using SPbSTU.OPD.ASAP.Core.Domain.Contracts.Services;
using SPbSTU.OPD.ASAP.Core.Domain.Models.Outbox.Points;
using SPbSTU.OPD.ASAP.Core.Infrastructure.Contracts;
using SPbSTU.OPD.ASAP.Core.Infrastructure.Kafka;

namespace SPbSTU.OPD.ASAP.Core.Kafka.Handlers;

public class PointsGithubHandler(IPointsService pointsService) : IHandler<Ignore, PointsGithubKafka>
{
    private readonly IPointsService _pointsService = pointsService;

    public async Task Handle(IReadOnlyCollection<ConsumeResult<Ignore, PointsGithubKafka>> messages,
        CancellationToken token)
    {
        var list = messages.Select(cr => MapToDomain(cr.Message.Value)).ToList();
        await _pointsService.CreatePointsAndQueue(list, token);
    }

    private static PointsGithub MapToDomain(PointsGithubKafka pointsGithub)
    {
        return new PointsGithub(pointsGithub.AssignmentTitle, pointsGithub.CourseTitle, pointsGithub.Username,
            pointsGithub.Date,
            pointsGithub.Points);
    }
}