using SPbSTU.OPD.ASAP.Core.Domain.Models;

namespace SPbSTU.OPD.ASAP.Core.Domain.Contracts.Repositories;

public interface ICoursesRepository
{
    Task<List<Course>> GetByUserId(long userId, CancellationToken ct);
    
    Task<List<Course>> GetCoursesByTitles(List<string> titles, CancellationToken ct);
}