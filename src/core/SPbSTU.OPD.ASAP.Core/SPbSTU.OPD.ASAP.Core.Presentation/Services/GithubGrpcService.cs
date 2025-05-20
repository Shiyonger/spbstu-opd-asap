using Grpc.Core;
using SPbSTU.OPD.ASAP.Core.Domain.Contracts.Services;
using SPbSTU.OPD.ASAP.Core.Presentation;
using Repository = SPbSTU.OPD.ASAP.Core.Domain.Models.Repository;

namespace SPbSTU.OPD.ASAP.Core.Services;

public class GithubGrpcService(IGithubService githubService) : GithubService.GithubServiceBase
{
    private readonly IGithubService _githubService = githubService;

    public override async Task<GetUsersToInviteResponse> GetUsersToInvite(GetUsersToInviteRequest request,
        ServerCallContext context)
    {
        var usernames =
            await _githubService.GetUsernamesToInvite(request.GithubOrganizationName, context.CancellationToken);
        return new GetUsersToInviteResponse { Usernames = { usernames } };
    }

    public override async Task<GetUsersToCreateRepositoryResponse> GetUsersToCreateRepository(
        GetUsersToCreateRepositoryRequest request, ServerCallContext context)
    {
        var usernames =
            await _githubService.GetUsernamesToCreateRepository(request.GithubOrganizationName, request.AssignmentTitle,
                context.CancellationToken);
        return new GetUsersToCreateRepositoryResponse { Usernames = { usernames } };
    }

    public override async Task<CreateRepositoriesResponse> CreateRepositories(CreateRepositoriesRequest request,
        ServerCallContext context)
    {
        var repositories = request.Repositories.Select(MapToDomain).ToList();
        var ids = await _githubService.CreateRepositories(repositories, context.CancellationToken);

        return new CreateRepositoriesResponse { RepositoryIds = { ids } };
    }

    public override async Task<MarkInvitedResponse> MarkInvited(MarkInvitedRequest request, ServerCallContext context)
    {
        await _githubService.MarkInvited(request.Usernames.ToList(), context.CancellationToken);
        return new MarkInvitedResponse();
    }

    private static Repository MapToDomain(Presentation.Repository repository)
    {
        return new Repository(repository.Username, repository.GithubOrganizationName, repository.AssignmentTitle,
            repository.RepositoryLink);
    }
}