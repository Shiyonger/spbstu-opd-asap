using SPbSTU.OPD.ASAP.Core.Domain.Contracts.Repositories;
using SPbSTU.OPD.ASAP.Core.Domain.Contracts.Services;
using SPbSTU.OPD.ASAP.Core.Domain.Models;

namespace SPbSTU.OPD.ASAP.Core.Application.Services;

public class GithubService(IGithubRepository githubRepository) : IGithubService
{
    private readonly IGithubRepository _githubRepository = githubRepository;

    public Task<List<string>> GetUsernamesToInvite(string githubOrganization, CancellationToken ct)
    {
        return _githubRepository.GetUsernamesToInvite(githubOrganization, ct);
    }

    public Task<List<string>> GetUsernamesToCreateRepository(string githubOrganization, string assignmentTitle, CancellationToken ct)
    {
        return _githubRepository.GetUsernamesToCreateRepository(githubOrganization, assignmentTitle, ct);
    }

    public Task<List<long>> CreateRepositories(List<Repository> repositories, CancellationToken ct)
    {
        return _githubRepository.CreateRepositories(repositories, ct);
    }

    public Task MarkInvited(List<string> usernames, CancellationToken ct)
    {
        return _githubRepository.MarkInvited(usernames, ct);
    }
}