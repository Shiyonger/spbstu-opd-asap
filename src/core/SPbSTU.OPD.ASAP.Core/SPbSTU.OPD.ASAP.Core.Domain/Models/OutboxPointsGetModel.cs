using SPbSTU.OPD.ASAP.Core.Domain.ValueObjects;

namespace SPbSTU.OPD.ASAP.Core.Domain.Models;

public record OutboxPointsGetModel(
    long Id,
    int Points,
    DateTime Date,
    long CourseId,
    Position StudentPosition,
    Position AssignmentPosition);