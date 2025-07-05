using Grpc.Core;
using SPbSTU.OPD.ASAP.Google.Application.Commands.CreateSpreadSheets;
using SPbSTU.OPD.ASAP.Google.Application.Interfaces;
using SPbSTU.OPD.ASAP.Google.Domain.Entities;
using SPbSTU.OPD.ASAP.Google.Domain.Interfaces;
using SPbSTU.OPD.ASAP.Google.Presentation;

namespace SPbSTU.OPD.ASAP.Google.Services;

public class SpreadSheetsGrpcService(ISpreadSheetService spreadSheetService)
    : SpreadSheetsService.SpreadSheetsServiceBase
{
    public override async Task<CreateSpreadSheetsResponse> CreateSpreadSheets(CreateSpreadSheetsRequest request, ServerCallContext context)
    {
        CreateSpreadSheetsCommand command = MapToDomain(request);

        CreateSpreadSheetsResult result = await spreadSheetService.CreateSpreadSheetsAsync(command, context.CancellationToken);

        return MapToGrpc(result);
    }
    
    private CreateSpreadSheetsCommand MapToDomain(CreateSpreadSheetsRequest request)
    {
        var courses = request.CourseList.Select(c => new Course(
            c.Id,
            c.Title,
            c.StudentList.Select(s => new Student(s.Id, s.Name)).ToList(),
            c.AssignmentList.Select(a => new Assignment(a.Id, a.Title)).ToList()
        )).ToList();

        return new CreateSpreadSheetsCommand { Courses = courses };
    }
    
    private CreateSpreadSheetsResponse MapToGrpc(CreateSpreadSheetsResult result)
    {
        var response = new CreateSpreadSheetsResponse();

        foreach (var course in result.CoursePositions)
        {
            var courseGrpc = new CoursePositionGrpc
            {
                Id = course.Id,
                Title = course.Title,
                SpreadsheetId = course.SpreadsheetId,
                PointsSpreadsheetLink = course.PointsSpreadsheetLink
            };

            courseGrpc.StudentPositionList.AddRange(course.StudentPositions.Select(s => new StudentPositionGrpc
            {
                Id = s.Id,
                Name = s.Name,
                Cell = s.Cell
            }));

            courseGrpc.AssignmentPositionList.AddRange(course.AssignmentPositions.Select(a => new AssignmentPositionGrpc
            {
                Id = a.Id,
                Title = a.Title,
                Cell = a.Cell,
                QueueSpreadsheetLink = a.QueueSpreadsheetLink,
                QueueSpreadsheetId = a.QueueSpreadsheetId
            }));

            response.CoursePositionList.Add(courseGrpc);
        }

        return response;
    }
}