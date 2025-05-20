using SPbSTU.OPD.ASAP.API.Domain.Contracts.Grpc;

namespace SPbSTU.OPD.ASAP.API.Infrastructure.Grpc;

public class CoursesGrpcService(CoursesService.CoursesServiceClient coursesClient) : ICoursesGrpcService
{
    private readonly CoursesService.CoursesServiceClient _coursesClient = coursesClient;

    public async Task<List<Domain.Models.Course>> GetCourses(long userId, CancellationToken ct)
    {
        var result =
            await _coursesClient.GetCoursesAsync(new GetCoursesRequest { UserId = userId }, cancellationToken: ct);
        if (result is null)
            return [];

        var courses = result.CoursesList.Select(MapToDomain);
        return courses.ToList();
    }

    private static Domain.Models.Course MapToDomain(Course courseGrpc)
    {
        return new Domain.Models.Course(courseGrpc.Id, courseGrpc.Title, courseGrpc.SubjectTitle,
            courseGrpc.GithubOrganizationLink);
    }
}