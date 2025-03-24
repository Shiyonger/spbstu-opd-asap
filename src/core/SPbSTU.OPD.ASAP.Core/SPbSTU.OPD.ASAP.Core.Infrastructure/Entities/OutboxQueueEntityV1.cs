namespace SPbSTU.OPD.ASAP.Core.Infrastructure.Entities;

public record OutboxQueueEntityV1
{
    public long Id { get; init; }
    public string Link { get; init; } = null!;
    public long MentorId { get; init; }
    public long AssignmentId { get; init; }
    public long SubmissionId { get; init; }
    public int Action { get; init; }
    public bool IsSent { get; init; }
}