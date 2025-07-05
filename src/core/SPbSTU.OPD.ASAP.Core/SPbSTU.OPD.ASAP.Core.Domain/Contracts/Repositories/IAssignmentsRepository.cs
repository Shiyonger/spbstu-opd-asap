using SPbSTU.OPD.ASAP.Core.Domain.Models;

namespace SPbSTU.OPD.ASAP.Core.Domain.Contracts.Repositories;

public interface IAssignmentsRepository
{
    Task<List<Assignment>> GetByCourseIdForMentor(long userId, long courseId, CancellationToken ct);
    
    Task<List<Assignment>> GetByCourseIdForStudent(long userId, long courseId, CancellationToken ct);
    
    Task<List<Assignment>> GetByCourseId(long courseId, CancellationToken ct);
    
    Task UpdateSpreadSheet(List<Assignment> assignments, CancellationToken ct);
}