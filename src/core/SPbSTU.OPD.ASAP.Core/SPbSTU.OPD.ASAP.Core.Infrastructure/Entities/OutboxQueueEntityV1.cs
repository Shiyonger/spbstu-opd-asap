namespace SPbSTU.OPD.ASAP.Core.Infrastructure.Entities;

public record OutboxQueueEntityV1(
    string Link,
    long MentorId,
    long AssignmentId,
    long SubmissionId,
    bool IsSent);