using SPbSTU.OPD.ASAP.Google.Domain.Entities;

namespace SPbSTU.OPD.ASAP.Google.Application.Commands.UpdateSpreadSheets;

public class UpdatePointsCommand
{
    public PointsMessage Message { get; init; } = null!;
}