using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using SPbSTU.OPD.ASAP.Google.Application.Interfaces;
using SPbSTU.OPD.ASAP.Google.Domain.Entities;
using SPbSTU.OPD.ASAP.Google.Domain.Interfaces;

namespace SPbSTU.OPD.ASAP.Google.Application.Services;

public sealed class UpdateSheetService(ISheetUpdater sheetUpdater) : IUpdateSheetService
{
    public Task UpdatePointsAsync(IReadOnlyCollection<PointsMessage> messages, CancellationToken cancellationToken)
    {
        foreach (var message in messages)
        {
            sheetUpdater.UpdatePoints(message);
        }

        return Task.CompletedTask;
    }

    public Task UpdateQueueAsync(IReadOnlyCollection<QueueMessage> messages, CancellationToken cancellationToken)
    {
        foreach (var message in messages)
        {
            sheetUpdater.UpdateQueue(message);
        }

        return Task.CompletedTask;
    }
}