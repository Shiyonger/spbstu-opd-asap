using SPbSTU.OPD.ASAP.Core.Domain.Contracts.Repositories;
using SPbSTU.OPD.ASAP.Core.Domain.Contracts.Services;
using SPbSTU.OPD.ASAP.Core.Domain.Enums;
using SPbSTU.OPD.ASAP.Core.Domain.Models;

namespace SPbSTU.OPD.ASAP.Core.Application.Services;

public class AssignmentsService(IAssignmentsRepository assignmentsRepository) : IAssignmentsService
{
    private readonly IAssignmentsRepository _assignmentsRepository = assignmentsRepository;

    public Task<List<Assignment>> GetByCourseId(long userId, long courseId, Role role, CancellationToken ct)
    {
        return role switch
        {
            Role.Student => _assignmentsRepository.GetByCourseIdForStudent(userId, courseId, ct),
            Role.Mentor => _assignmentsRepository.GetByCourseIdForMentor(userId, courseId, ct),
            _ => throw new ArgumentOutOfRangeException(nameof(role), role, null)
        };
    }
}