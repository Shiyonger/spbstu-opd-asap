using SPbSTU.OPD.ASAP.API.Domain.Contracts;
using SPbSTU.OPD.ASAP.API.Domain.Contracts.Grpc;
using SPbSTU.OPD.ASAP.API.Domain.Models;
using SPbSTU.OPD.ASAP.API.Domain.Results;

namespace SPbSTU.OPD.ASAP.API.Infrastructure.Grpc;

public class UsersGrpcService(UsersService.UsersServiceClient usersClient) : IUsersGrpcService
{
    private readonly UsersService.UsersServiceClient _usersClient = usersClient;

    public async Task<long> CreateUser(Domain.Models.User user, CancellationToken ct)
    {
        var userGrpc = new User
        {
            Name = user.Name, Login = user.Login, Password = user.Password, Email = user.Email,
            Role = (Role)(int)user.Role, GithubLink = user.GithubLink
        };

        var response =
            await _usersClient.CreateUserAsync(new CreateUserRequest { User = userGrpc }, cancellationToken: ct);

        return response.UserId;
    }

    public async Task<GrpcGetUserResult> GetUser(string login, CancellationToken ct)
    {
        var response = await _usersClient.GetUserAsync(new GetUserRequest { Login = login }, cancellationToken: ct);

        if (!response.IsFound)
            return new GrpcGetUserResult { IsSuccessful = false, ErrorMessage = "User not found" };

        var user = new Domain.Models.User(response.UserId, response.User.Name, response.User.Login,
            response.User.Password,
            response.User.Email, (Domain.Enums.Role)(int)response.User.Role, response.User.GithubLink);

        return new GrpcGetUserResult { IsSuccessful = true, User = user };
    }
}