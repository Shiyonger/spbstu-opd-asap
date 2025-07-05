using Confluent.Kafka;
using SPbSTU.OPD.ASAP.Google.Application.Interfaces;
using SPbSTU.OPD.ASAP.Google.Domain.Entities;
using SPbSTU.OPD.ASAP.Google.Domain.ValueObjects;
using SPbSTU.OPD.ASAP.Google.Infrastructure.Contracts;
using SPbSTU.OPD.ASAP.Google.Infrastructure.Kafka;

namespace SPbSTU.OPD.ASAP.Google.Kafka;

public class PointsHandler : IHandler<Ignore, PointsGoogleKafka>
{
    private readonly IUpdateSheetService _updateSheetService;
    public PointsHandler(IUpdateSheetService updateSheetService)
    {
        _updateSheetService = updateSheetService;
    }
    
    public async Task Handle(IReadOnlyCollection<ConsumeResult<Ignore, PointsGoogleKafka>> messages, CancellationToken token)
    {
        var pointsMessages = messages
            .Select(m => MapToDomain(m.Message.Value))
            .ToList();

        await _updateSheetService.UpdatePointsAsync(pointsMessages, token);
    }
    
    private static PointsMessage MapToDomain(PointsGoogleKafka kafka)
    {
        return new PointsMessage(
            kafka.Id,
            kafka.Points,
            kafka.Date,
            new Position(
                kafka.StudentPosition.Cell,
                kafka.StudentPosition.SpreadSheetId),
            new Position(
                kafka.AssignmentPosition.Cell,
                kafka.AssignmentPosition.SpreadSheetId)
        );
    }
}