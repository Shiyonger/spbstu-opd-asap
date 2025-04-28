using SPbSTU.OPD.ASAP.Core.Domain.Models.Course;

namespace SPbSTU.OPD.ASAP.Core.Domain.Contracts.Repositories;

public interface ICoursesRepository
{
    Task<List<Course>> GetByUserId(long userId, CancellationToken ct);
}