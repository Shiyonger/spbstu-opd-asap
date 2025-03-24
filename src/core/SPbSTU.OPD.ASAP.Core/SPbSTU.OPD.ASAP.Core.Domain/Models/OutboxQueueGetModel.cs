namespace SPbSTU.OPD.ASAP.Core.Domain.Models;

public record OutboxQueueGetModel(
    long Id,
    string Link,
    long MentorId,
    string MentorName,
    long AssignmentId,
    string AssignmentTitle,
    long SubmissionId,
    DateTime SubmissionDate,
    int Action);