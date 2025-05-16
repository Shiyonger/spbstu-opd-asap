using SPbSTU.OPD.ASAP.Google.Domain.Entities;

namespace SPbSTU.OPD.ASAP.Google.Application.Commands.CreateSpreadSheets;

public sealed class CreateSpreadSheetsCommand
{
    public IReadOnlyList<Student> Students { get; init; } = [];
    public IReadOnlyList<Assignment> Assignments { get; init; } = [];
}