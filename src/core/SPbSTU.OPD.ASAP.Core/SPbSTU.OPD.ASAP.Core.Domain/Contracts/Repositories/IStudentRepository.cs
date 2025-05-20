using SPbSTU.OPD.ASAP.Core.Domain.Models;

namespace SPbSTU.OPD.ASAP.Core.Domain.Contracts.Repositories;

public interface IStudentRepository
{
    Task<List<Student>> GetByCourseId(long courseId, CancellationToken ct);
}