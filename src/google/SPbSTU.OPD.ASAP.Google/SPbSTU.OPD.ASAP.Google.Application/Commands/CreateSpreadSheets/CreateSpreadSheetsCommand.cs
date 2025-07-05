using SPbSTU.OPD.ASAP.Google.Domain.Entities;

namespace SPbSTU.OPD.ASAP.Google.Application.Commands.CreateSpreadSheets;

public sealed class CreateSpreadSheetsCommand
{
    public IReadOnlyList<Course> Courses { get; init; } = [];
}