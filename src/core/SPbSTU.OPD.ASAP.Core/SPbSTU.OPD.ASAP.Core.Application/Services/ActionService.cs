using SPbSTU.OPD.ASAP.Core.Domain.Contracts.Repositories;
using SPbSTU.OPD.ASAP.Core.Domain.Contracts.Services;
using SPbSTU.OPD.ASAP.Core.Domain.Models.Outbox.Queue;

namespace SPbSTU.OPD.ASAP.Core.Application.Services;

public class ActionService(
    IOutboxQueueRepository queueRepository,
    ISubmissionsRepository submissionsRepository) : OutboxService(submissionsRepository), IActionService
{
    private readonly IOutboxQueueRepository _queueRepository = queueRepository;

    public async Task CreateQueueAndSubmissions(List<ActionGithub> actionGithubs, CancellationToken token)
    {
        using var transaction = CreateTransactionScope();

        var actionsToCreate = actionGithubs.Where(a => a.Action == Domain.ValueObjects.Action.Create).ToList();
        if (actionsToCreate.Count != 0)
            await CreateSubmissions(actionsToCreate, token);

        var actionsToUpdate = actionGithubs.Where(a => a.Action == Domain.ValueObjects.Action.Update).ToList();
        if (actionsToUpdate.Count != 0)
            await UpdateSubmissions(actionsToUpdate, token);

        var submissions = await GetSubmissions(actionGithubs, token);
        var queueModels = actionGithubs.Select(a =>
        {
            var submission = submissions[(a.Username, a.AssignmentTitle)];
            return new OutboxQueueCreateModel(submission.RepositoryLink, submission.MentorId, submission.AssignmentId,
                submission.Id, a.Action);
        }).ToList();
        await _queueRepository.Create(queueModels, token);

        transaction.Complete();
    }

    public Task<List<OutboxQueueGetModel>> GetNotSentQueue(CancellationToken token)
    {
        return _queueRepository.GetNotSent(token);
    }

    public Task UpdateSentQueue(List<long> queueQueryIds, CancellationToken token)
    {
        return _queueRepository.UpdateSent(queueQueryIds, token);
    }

    private Task<List<long>> CreateSubmissions(List<ActionGithub> actionGithubs, CancellationToken token)
    {
        var (usernames, assignmentTitles) = GetUsernamesAndTitles(actionGithubs);
        return SubmissionsRepository.CreateSubmissions(usernames, assignmentTitles, token);
    }

    private async Task UpdateSubmissions(List<ActionGithub> actionGithubs, CancellationToken token)
    {
        var submissionsIdsToUpdate = (await GetSubmissions(actionGithubs, token))
            .Values.Select(s => s.Id).ToList();
        await SubmissionsRepository.UpdateSubmissions(submissionsIdsToUpdate, token);
    }
}