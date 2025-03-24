namespace SPbSTU.OPD.ASAP.Core.Domain.Models;

public record OutboxQueueGetModel
{
    public long Id { get; init; }
    public string Link { get; init; } = null!;
    public long MentorId { get; init; }
    public string MentorName { get; init; } = null!;
    public long AssignmentId { get; init; }
    public string AssignmentTitle { get; init; } = null!;
    public long SubmissionId { get; init; }
    public DateTime SubmissionDate { get; init; }
    public int Action { get; init; }
}