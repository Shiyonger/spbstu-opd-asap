using SPbSTU.OPD.ASAP.API.Domain.Enums;
using SPbSTU.OPD.ASAP.API.Domain.Models;

namespace SPbSTU.OPD.ASAP.API.Domain.Contracts.Grpc;

public interface IAssignmentsGrpcService
{
    Task<List<Assignment>> GetAssignments(long userId, long courseId, Role role, CancellationToken ct);
}