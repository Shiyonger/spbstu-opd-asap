using SPbSTU.OPD.ASAP.Google.Domain.ValueObjects;

namespace SPbSTU.OPD.ASAP.Google.Application.Commands.CreateSpreadSheets;

public record CreateSpreadSheetsResult(
    IReadOnlyList<StudentPosition> StudentPositions,
    IReadOnlyList<AssignmentPosition> AssignmentPositions);
