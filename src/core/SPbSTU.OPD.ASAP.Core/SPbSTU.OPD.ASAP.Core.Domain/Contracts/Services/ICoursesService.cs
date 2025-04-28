using SPbSTU.OPD.ASAP.Core.Domain.Models.Course;

namespace SPbSTU.OPD.ASAP.Core.Domain.Contracts.Services;

public interface ICoursesService
{
    Task<List<Course>> GetByUserId(long userId, CancellationToken ct);
}