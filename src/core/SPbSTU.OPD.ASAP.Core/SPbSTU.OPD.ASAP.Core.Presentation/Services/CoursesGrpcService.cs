using Grpc.Core;
using SPbSTU.OPD.ASAP.Core.Domain.Contracts.Services;
using SPbSTU.OPD.ASAP.Core.Presentation;

namespace SPbSTU.OPD.ASAP.Core.Services;

public class CoursesGrpcService(ICoursesService coursesService) : CoursesService.CoursesServiceBase
{
    private readonly ICoursesService _coursesService = coursesService;

    public override async Task<GetCoursesResponse> GetCourses(GetCoursesRequest request, ServerCallContext context)
    {
        var result = await _coursesService.GetByUserId(request.UserId, context.CancellationToken);
        var courses = result.Select(MapToGrpc).ToList();

        return new GetCoursesResponse { CoursesList = { courses } };
    }

    private static Course MapToGrpc(Domain.Models.Course course)
    {
        return new Course
        {
            Id = course.Id, Title = course.Title, SubjectTitle = course.SubjectTitle,
            GithubOrganizationLink = course.GithubOrganizationLink, GoogleSpreadsheetLink = course.GoogleSpreadSheetLink
        };
    }
}