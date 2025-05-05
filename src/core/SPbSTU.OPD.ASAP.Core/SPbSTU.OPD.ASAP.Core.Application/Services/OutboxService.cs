using System.Transactions;
using SPbSTU.OPD.ASAP.Core.Domain.Contracts.Repositories;
using SPbSTU.OPD.ASAP.Core.Domain.Contracts.Services;
using SPbSTU.OPD.ASAP.Core.Domain.Models;
using SPbSTU.OPD.ASAP.Core.Domain.Models.Outbox;

namespace SPbSTU.OPD.ASAP.Core.Application.Services;

public class OutboxService(
    ISubmissionsRepository submissionsRepository)
    : IOutboxService
{
    protected readonly ISubmissionsRepository SubmissionsRepository = submissionsRepository;

    protected Task<Dictionary<(string Username, string Title), Submission>> GetSubmissions<T>(List<T> input,
        CancellationToken token) where T : Github
    {
        var (usernames, assignmentTitles) = GetUsernamesAndTitles(input);
        return SubmissionsRepository.GetByUsernameAndAssignment(usernames, assignmentTitles, token);
    }

    protected static (List<string> usernames, List<string> assignmentTitles) GetUsernamesAndTitles<T>(List<T> input)
        where T : Github
    {
        var usernames = new List<string>();
        var assignmentTitles = new List<string>();
        foreach (var p in input)
        {
            usernames.Add(p.Username);
            assignmentTitles.Add(p.AssignmentTitle);
        }

        return (usernames, assignmentTitles);
    }

    protected static TransactionScope CreateTransactionScope(
        IsolationLevel level = IsolationLevel.ReadCommitted)
    {
        return new TransactionScope(
            TransactionScopeOption.Required,
            new TransactionOptions
            {
                IsolationLevel = level,
                Timeout = TimeSpan.FromSeconds(5)
            },
            TransactionScopeAsyncFlowOption.Enabled);
    }
}