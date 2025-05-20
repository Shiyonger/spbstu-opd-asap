using SPbSTU.OPD.ASAP.Core.Domain.Contracts.Services;
using SPbSTU.OPD.ASAP.Core.Domain.Models;
using SPbSTU.OPD.ASAP.Core.Domain.ValueObjects;
using SPbSTU.OPD.ASAP.Core.Presentation;
using Course = SPbSTU.OPD.ASAP.Core.Domain.Models.Course;

namespace SPbSTU.OPD.ASAP.Core.Services;

public class SpreadSheetsBackgroundService(
    IServiceScopeFactory scopeFactory,
    ILogger<SpreadSheetsBackgroundService> logger,
    SpreadSheetsService.SpreadSheetsServiceClient client)
    : IHostedService, IDisposable
{
    private Timer? _timer;
    private readonly IServiceScopeFactory _scopeFactory = scopeFactory;
    private readonly SpreadSheetsService.SpreadSheetsServiceClient _client = client;
    private readonly ILogger<SpreadSheetsBackgroundService> _logger = logger;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));

        return Task.CompletedTask;
    }

    private async void DoWork(object? state)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var coursesService = scope.ServiceProvider.GetRequiredService<ICoursesService>();
            using var cts = new CancellationTokenSource();
            var token = cts.Token;

            var courses = await coursesService.GetForCreateSpreadSheet(token);
            var result = await _client.CreateSpreadSheetsAsync(new CreateSpreadSheetsRequest
                { CourseList = { courses.Select(MapToGrpc).ToList() } });

            var coursesResult = result.CoursePositionList.Select(MapToDomain).ToList();
            await coursesService.UpdateSpreadSheets(coursesResult, token);
        }
        catch (Exception e)
        {
            _logger.LogCritical(e.Message);
        }
    }

    private static CourseGrpc MapToGrpc(Course course)
    {
        return new CourseGrpc
        {
            Id = course.Id, Title = course.Title,
            StudentList = { course.Students!.Select(s => new StudentGrpc { Id = s.Id, Name = s.Name }).ToList() },
            AssignmentList = { course.Assignments!.Select(a => new AssignmentGrpc { Id = a.Id, Title = a.Title }) }
        };
    }

    private static Course MapToDomain(CoursePositionGrpc course)
    {
        return new Course
        {
            Id = course.Id, Title = course.Title, GoogleSpreadSheetLink = course.PointsSpreadsheetLink,
            Students = course.StudentPositionList.Select(s => new Student
                { Id = s.Id, Name = s.Name, Position = new Position(s.Cell, course.SpreadsheetId) }).ToList(),
            Assignments = course.AssignmentPositionList.Select(a => new Domain.Models.Assignment
            {
                Id = a.Id, Title = a.Title, Link = a.QueueSpreadsheetLink, SpreadSheetId = a.QueueSpreadsheetId,
                Position = new Position(a.Cell, course.SpreadsheetId)
            }).ToList()
        };
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}