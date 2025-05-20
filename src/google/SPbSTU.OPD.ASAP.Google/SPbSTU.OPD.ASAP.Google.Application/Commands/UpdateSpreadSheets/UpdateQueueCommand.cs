using SPbSTU.OPD.ASAP.Google.Domain.Entities;

namespace SPbSTU.OPD.ASAP.Google.Application.Commands.UpdateSpreadSheets;

public class UpdateQueueCommand
{
    public QueueMessage Message { get; init; } = null!;
}