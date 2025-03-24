namespace SPbSTU.OPD.ASAP.Core.Infrastructure.Entities;

public record OutboxQueueEntityV1(
    long Id,
    string Link,
    long MentorId,
    long AssignmentId,
    long SubmissionId,
    int Action,
    bool IsSent);