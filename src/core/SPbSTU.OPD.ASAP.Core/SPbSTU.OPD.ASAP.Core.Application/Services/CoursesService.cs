using SPbSTU.OPD.ASAP.Core.Domain.Contracts.Repositories;
using SPbSTU.OPD.ASAP.Core.Domain.Contracts.Services;
using SPbSTU.OPD.ASAP.Core.Domain.Models.Course;

namespace SPbSTU.OPD.ASAP.Core.Application.Services;

public class CoursesService(ICoursesRepository coursesRepository) : ICoursesService
{
    private readonly ICoursesRepository _coursesRepository = coursesRepository;

    public Task<List<Course>> GetByUserId(long userId, CancellationToken ct)
    {
        return _coursesRepository.GetByUserId(userId, ct);
    }
}