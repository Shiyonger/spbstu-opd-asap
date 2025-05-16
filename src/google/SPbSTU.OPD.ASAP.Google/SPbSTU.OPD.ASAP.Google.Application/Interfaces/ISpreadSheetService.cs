using SPbSTU.OPD.ASAP.Google.Application.Commands.CreateSpreadSheets;

namespace SPbSTU.OPD.ASAP.Google.Application.Interfaces;

public interface ISpreadSheetService
{
    Task<CreateSpreadSheetsResult> CreateSpreadSheetsAsync(CreateSpreadSheetsCommand command, CancellationToken cancellationToken);
}