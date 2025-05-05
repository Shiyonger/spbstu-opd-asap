using SPbSTU.OPD.ASAP.Core.Domain.Models;

namespace SPbSTU.OPD.ASAP.Core.Domain.Contracts.Repositories;

public interface ISubmissionsRepository
{
    Task<Dictionary<(string Username, string Title), Submission>> GetByUsernameAndAssignment(
        List<string> usernames,
        List<string> assignmentTitles,
        CancellationToken ct);

    Task<List<long>> CreateSubmissions(
        List<string> usernames,
        List<string> assignmentTitles,
        CancellationToken ct);

    Task UpdateSubmissions(
        List<long> submissionIds,
        CancellationToken ct);
}