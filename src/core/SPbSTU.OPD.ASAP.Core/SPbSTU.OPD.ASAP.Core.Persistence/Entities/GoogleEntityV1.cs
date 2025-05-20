namespace SPbSTU.OPD.ASAP.Core.Persistence.Entities;

public class GoogleEntityV1()
{
    public long Id { get; init; }
    public long StudentId { get; init; }
    public long CourseId { get; init; }
    public long AssignmentId { get; init; }
    public long AssignmentPositionId { get; init; }
    public long StudentPositionId { get; init; }
}