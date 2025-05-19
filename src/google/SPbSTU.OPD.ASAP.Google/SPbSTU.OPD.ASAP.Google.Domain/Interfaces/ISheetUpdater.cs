using SPbSTU.OPD.ASAP.Google.Domain.Entities;

namespace SPbSTU.OPD.ASAP.Google.Domain.Interfaces;

public interface ISheetUpdater
{
    Task UpdatePointsAsync(PointsMessage message, CancellationToken cancellationToken);
    Task UpdateQueueAsync(QueueMessage message, CancellationToken cancellationToken);
}