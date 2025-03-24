namespace SPbSTU.OPD.ASAP.Core.Infrastructure.Entities;

public record OutboxPointsEntityV1()
{
    public long Id { get; init; }
    public int Points { get; init; }
    public DateTime Date { get; init; }
    public long CourseId { get; init; }
    public string StudentPosition { get; init; } = null!;
    public string AssignmentPosition { get; init; } = null!;
    public bool IsSent { get; init; }
}