namespace SPbSTU.OPD.ASAP.Core.Infrastructure.Entities;

public record OutboxPointsEntityV1(
    int Points,
    DateTime Date,
    long CourseId,
    string StudentPosition,
    string AssignmentPosition,
    bool IsSent);