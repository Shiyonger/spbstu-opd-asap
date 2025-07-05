using SPbSTU.OPD.ASAP.Core.Domain.Enums;
using SPbSTU.OPD.ASAP.Core.Domain.Models;

namespace SPbSTU.OPD.ASAP.Core.Domain.Contracts.Services;

public interface IAssignmentsService
{
    Task<List<Assignment>> GetByCourseId(long userId, long courseId, Role role, CancellationToken ct);
}