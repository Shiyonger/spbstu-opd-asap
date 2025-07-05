using SPbSTU.OPD.ASAP.Core.Domain.ValueObjects;

namespace SPbSTU.OPD.ASAP.Core.Domain.Models.Outbox.Points;

public record OutboxPointsCreateModel(
    int Points,
    DateTime Date,
    long CourseId,
    Position StudentPosition,
    Position AssignmentPosition);