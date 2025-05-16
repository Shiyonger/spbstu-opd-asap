using SPbSTU.OPD.ASAP.Google.Domain.ValueObjects;

namespace SPbSTU.OPD.ASAP.Google.Domain.Entities;

public sealed class SpreadSheetResult
{
    public SpreadSheetResult(
        long id,
        IReadOnlyList<StudentPosition> studentPositions,
        IReadOnlyList<AssignmentPosition> assignmentPositions)
    {
        Id = id;
        StudentPositions = studentPositions;
        AssignmentPositions = assignmentPositions;
    }

    public long Id { get; }

    public IReadOnlyList<StudentPosition> StudentPositions { get; }

    public IReadOnlyList<AssignmentPosition> AssignmentPositions { get; }
}