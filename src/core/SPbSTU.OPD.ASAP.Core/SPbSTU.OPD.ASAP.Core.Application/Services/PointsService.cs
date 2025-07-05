using SPbSTU.OPD.ASAP.Core.Domain.Contracts.Repositories;
using SPbSTU.OPD.ASAP.Core.Domain.Contracts.Services;
using SPbSTU.OPD.ASAP.Core.Domain.Models.Outbox.Points;
using SPbSTU.OPD.ASAP.Core.Domain.Models.Outbox.Queue;

namespace SPbSTU.OPD.ASAP.Core.Application.Services;

public class PointsService(
    IOutboxPointsRepository pointsRepository,
    IOutboxQueueRepository queueRepository,
    ICoursesRepository coursesRepository,
    IGoogleRepository googleRepository,
    ISubmissionsRepository submissionsRepository) : OutboxService(submissionsRepository), IPointsService
{
    private readonly IOutboxPointsRepository _pointsRepository = pointsRepository;
    private readonly IOutboxQueueRepository _queueRepository = queueRepository;
    private readonly ICoursesRepository _coursesRepository = coursesRepository;
    private readonly IGoogleRepository _googleRepository = googleRepository;

    public async Task CreatePointsAndQueue(List<PointsGithub> pointsGithub, CancellationToken token)
    {
        using var transaction = CreateTransactionScope();

        var submissions = await GetSubmissions(pointsGithub, token);

        var pointsModels = await GetPointsModel(pointsGithub, token);
        var queueModels = pointsGithub.Select(p =>
        {
            var submission = submissions[(p.Username, p.AssignmentTitle)];
            return new OutboxQueueCreateModel(submission.RepositoryLink, submission.MentorId, submission.AssignmentId,
                submission.Id, Domain.ValueObjects.Action.Delete);
        }).ToList();

        await _pointsRepository.Create(pointsModels, token);
        await _queueRepository.Create(queueModels, token);

        transaction.Complete();
    }

    public Task<List<OutboxPointsGetModel>> GetNotSentPoints(CancellationToken token)
    {
        return _pointsRepository.GetNotSent(token);
    }

    public Task UpdateSentPoints(List<long> sentPointsIds, CancellationToken token)
    {
        return _pointsRepository.UpdateSent(sentPointsIds, token);
    }

    private async Task<List<OutboxPointsCreateModel>> GetPointsModel(List<PointsGithub> pointsGithub,
        CancellationToken token)
    {
        var coursesMap =
            (await _coursesRepository.GetCoursesByTitles(pointsGithub.Select(p => p.CourseTitle).ToList(), token))
            .ToDictionary(c => c.Title);
        var studentsPositions =
            await _googleRepository.GetStudentsPositions(
                pointsGithub.Select(p => (p.Username, coursesMap[p.CourseTitle].Id)).ToList(), token);
        var assignmentsPositions =
            await _googleRepository.GetAssignmentsPositions(
                pointsGithub.Select(p => (p.AssignmentTitle, coursesMap[p.CourseTitle].Id)).ToList(), token);

        return pointsGithub.Select(p =>
                new OutboxPointsCreateModel(
                    p.Points,
                    p.Date,
                    coursesMap[p.CourseTitle].Id,
                    studentsPositions[(p.Username, coursesMap[p.CourseTitle].Id)],
                    assignmentsPositions[(p.AssignmentTitle, coursesMap[p.CourseTitle].Id)]))
            .ToList();
    }
}