using SPbSTU.OPD.ASAP.API.Domain.Contracts.Grpc;
using SPbSTU.OPD.ASAP.API.Domain.Models;

namespace SPbSTU.OPD.ASAP.API.Infrastructure.Grpc;

public class AssignmentsGrpcService(AssignmentsService.AssignmentsServiceClient assignmentsClient)
    : IAssignmentsGrpcService
{
    private readonly AssignmentsService.AssignmentsServiceClient _assignmentsClient = assignmentsClient;

    public async Task<List<Domain.Models.Assignment>> GetAssignments(long userId, long courseId, Domain.Enums.Role role, CancellationToken ct)
    {
        var result = await _assignmentsClient.GetAssignmentsAsync(
            new GetAssignmentsRequest { UserId = userId, CourseId = courseId, Role = (Role)(int)role },
            cancellationToken: ct);
        if (result is null) 
            return [];

        var assignments = result.AssignmentsList.Select(MapToDomain);
        return assignments.ToList();
    }

    private static Domain.Models.Assignment MapToDomain(Assignment assignmentGrpc)
    {
        return new Domain.Models.Assignment(assignmentGrpc.Id, assignmentGrpc.Title, assignmentGrpc.Description,
            assignmentGrpc.MaxPoints, assignmentGrpc.DueTo.ToDateTimeOffset(), assignmentGrpc.Link);
    }
}