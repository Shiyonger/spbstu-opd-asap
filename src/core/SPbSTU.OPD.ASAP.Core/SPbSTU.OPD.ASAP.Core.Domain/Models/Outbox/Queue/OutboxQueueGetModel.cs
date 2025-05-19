namespace SPbSTU.OPD.ASAP.Core.Domain.Models.Outbox.Queue;

public record OutboxQueueGetModel
{
    public long Id { get; init; }
    public string Link { get; init; } = null!;
    public long StudentId { get; init; }
    public string StudentName { get; init; } = null!;
    public long GroupId { get; init; }
    public string SpreadsheetId { get; init; } = null!;
    public long SubmissionId { get; init; }
    public DateTime SubmissionDate { get; init; }
    public int Action { get; init; }
}