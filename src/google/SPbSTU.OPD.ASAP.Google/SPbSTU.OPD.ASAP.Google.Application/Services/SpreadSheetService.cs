using SPbSTU.OPD.ASAP.Google.Application.Commands.CreateSpreadSheets;
using SPbSTU.OPD.ASAP.Google.Application.Interfaces;
using SPbSTU.OPD.ASAP.Google.Domain.Entities;
using SPbSTU.OPD.ASAP.Google.Domain.Interfaces;
using SPbSTU.OPD.ASAP.Google.Domain.ValueObjects;

namespace SPbSTU.OPD.ASAP.Google.Application.Services;

public sealed class SpreadSheetService(ISpreadSheetBuilder builder) : ISpreadSheetService
{
    public async Task<CreateSpreadSheetsResult> CreateSpreadSheetsAsync(
        CreateSpreadSheetsCommand command,
        CancellationToken cancellationToken)
    {
        var coursePositions = new List<CoursePosition>();

        foreach (var course in command.Courses)
        {
            var coursePosition = await builder.BuildAsync(course, cancellationToken);
            coursePositions.Add(coursePosition);
        }

        return new CreateSpreadSheetsResult(coursePositions);
    }
}