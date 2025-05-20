using SPbSTU.OPD.ASAP.Google.Domain.ValueObjects;

namespace SPbSTU.OPD.ASAP.Google.Domain.Entities;

public record PointsMessage(
    long Id, 
    int Points, 
    DateTime Date,
    Position StudentPosition,
    Position AssignmentPosition
    );