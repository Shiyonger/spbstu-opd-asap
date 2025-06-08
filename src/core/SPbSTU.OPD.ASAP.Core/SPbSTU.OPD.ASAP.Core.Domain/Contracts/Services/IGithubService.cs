using SPbSTU.OPD.ASAP.Core.Domain.Models;

namespace SPbSTU.OPD.ASAP.Core.Domain.Contracts.Services;

public interface IGithubService
{
    Task<List<string>> GetUsernamesToInvite(string githubOrganization, CancellationToken ct);

    Task<List<string>> GetUsernamesToCreateRepository(string githubOrganization, string assignmentTitle,
        CancellationToken ct);

    Task<List<long>> CreateRepositories(List<Repository> repositories, CancellationToken ct);
    
    Task MarkInvited(List<string> usernames, CancellationToken ct);
    
    Task<List<string>> GetOrganizations(CancellationToken ct);
}