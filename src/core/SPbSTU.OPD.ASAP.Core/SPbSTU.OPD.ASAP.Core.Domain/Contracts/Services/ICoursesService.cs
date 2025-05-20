using SPbSTU.OPD.ASAP.Core.Domain.Models;

namespace SPbSTU.OPD.ASAP.Core.Domain.Contracts.Services;

public interface ICoursesService
{
    Task<List<Course>> GetByUserId(long userId, CancellationToken ct);
    
    Task<List<Course>> GetForCreateSpreadSheet(CancellationToken ct);
    
    Task UpdateSpreadSheets(List<Course> courses,  CancellationToken ct);
}