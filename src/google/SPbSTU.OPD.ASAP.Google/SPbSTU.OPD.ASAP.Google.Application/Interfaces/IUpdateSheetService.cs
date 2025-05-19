using SPbSTU.OPD.ASAP.Google.Domain.Entities;

namespace SPbSTU.OPD.ASAP.Google.Application.Interfaces;

public interface IUpdateSheetService
{
    Task UpdatePointsAsync(IReadOnlyCollection<PointsMessage> messages, CancellationToken cancellationToken);
}