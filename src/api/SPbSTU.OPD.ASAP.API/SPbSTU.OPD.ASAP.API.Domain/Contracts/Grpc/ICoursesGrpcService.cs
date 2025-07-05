using SPbSTU.OPD.ASAP.API.Domain.Models;

namespace SPbSTU.OPD.ASAP.API.Domain.Contracts.Grpc;

public interface ICoursesGrpcService
{
    Task<List<Course>> GetCourses(long userId, CancellationToken ct);
}