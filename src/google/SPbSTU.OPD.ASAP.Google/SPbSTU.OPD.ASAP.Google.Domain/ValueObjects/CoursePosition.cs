using SPbSTU.OPD.ASAP.Google.Domain.Entities;
namespace SPbSTU.OPD.ASAP.Google.Domain.ValueObjects;

public record CoursePosition(
    long Id,
    string Title,
    string SpreadsheetId,
    string PointsSpreadsheetLink,
    IReadOnlyList<StudentPosition> StudentPositions, 
    IReadOnlyList<AssignmentPosition> AssignmentPositions);
