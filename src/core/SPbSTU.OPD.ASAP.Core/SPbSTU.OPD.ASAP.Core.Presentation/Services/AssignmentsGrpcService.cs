using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using SPbSTU.OPD.ASAP.Core.Domain.Contracts.Services;
using SPbSTU.OPD.ASAP.Core.Presentation;
using Role = SPbSTU.OPD.ASAP.Core.Domain.Enums.Role;

namespace SPbSTU.OPD.ASAP.Core.Services;

public class AssignmentsGrpcService(IAssignmentsService assignmentsService) : AssignmentsService.AssignmentsServiceBase
{
    private readonly IAssignmentsService _assignmentsService = assignmentsService;

    public override async Task<GetAssignmentsResponse> GetAssignments(GetAssignmentsRequest request,
        ServerCallContext context)
    {
        var result = await _assignmentsService.GetByCourseId(
            request.UserId, request.CourseId, (Role)(int)request.Role,
            context.CancellationToken);

        var assignments = result.Select(MapToGrpc).ToList();
        return new GetAssignmentsResponse { AssignmentsList = { assignments } };
    }

    private static Assignment MapToGrpc(Domain.Models.Assignment assignment)
    {
        return new Assignment
        {
            Id = assignment.Id, Title = assignment.Title, Description = assignment.Description,
            MaxPoints = assignment.MaxPoints!.Value, DueTo = Timestamp.FromDateTimeOffset(assignment.DueTo!.Value),
            Link = assignment.Link
        };
    }
}