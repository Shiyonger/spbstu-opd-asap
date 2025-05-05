using SPbSTU.OPD.ASAP.Core.Domain.Models;

namespace SPbSTU.OPD.ASAP.Core.Domain.Contracts.Services;

public interface ICoursesService
{
    Task<List<Course>> GetByUserId(long userId, CancellationToken ct);
}