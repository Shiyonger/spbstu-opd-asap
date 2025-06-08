using SPbSTU.OPD.ASAP.Google.Domain.Entities;

namespace SPbSTU.OPD.ASAP.Google.Domain.Interfaces;

public interface ISheetUpdater
{
    void UpdatePoints(PointsMessage message);
    void UpdateQueue(QueueMessage message);
}