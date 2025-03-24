using SPbSTU.OPD.ASAP.Core.Domain.ValueObjects;

namespace SPbSTU.OPD.ASAP.Core.Domain.Models;

public record OutboxPoints(
    int Points,
    DateTime Date,
    long CourseId,
    Position StudentPosition,
    Position AssignmentPosition);