namespace SPbSTU.OPD.ASAP.Core.Persistence.Entities;

public record OutboxPointsEntityV1()
{
    public long Id { get; init; }
    public int Points { get; init; }
    public DateTime Date { get; init; }
    public long CourseId { get; init; }
    public string StudentPositionCell { get; init; } = null!;
    public string StudentPositionSpreadsheetId { get; init; } = null!;
    public string AssignmentPositionCell { get; init; } = null!;
    public string AssignmentPositionSpreadsheetId { get; init; } = null!;
    public bool IsSent { get; init; }
}