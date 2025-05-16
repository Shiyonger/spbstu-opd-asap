using SPbSTU.OPD.ASAP.Google.Application.Commands.CreateSpreadSheets;
using SPbSTU.OPD.ASAP.Google.Application.Interfaces;
using SPbSTU.OPD.ASAP.Google.Domain.Entities;
using SPbSTU.OPD.ASAP.Google.Domain.Interfaces;
using SPbSTU.OPD.ASAP.Google.Domain.ValueObjects;

namespace SPbSTU.OPD.ASAP.Google.Application.Services;

public sealed class SpreadSheetService : ISpreadSheetService
{
    private readonly ISpreadSheetBuilder _builder;

    public SpreadSheetService(
        ISpreadSheetBuilder builder)
    {
        _builder = builder;
    }

    public async Task<CreateSpreadSheetsResult> CreateSpreadSheetsAsync(
        CreateSpreadSheetsCommand command,
        CancellationToken cancellationToken)
    {
        SpreadSheetResult sheetResult = await _builder.BuildAsync(
            command.Students,
            command.Assignments,
            cancellationToken);

        return new CreateSpreadSheetsResult(
            sheetResult.StudentPositions
                .ToArray(),
            sheetResult.AssignmentPositions
                .ToArray()
        );
    }
}