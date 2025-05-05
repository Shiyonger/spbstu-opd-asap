using Grpc.Core;
using SPbSTU.OPD.ASAP.Core.Domain.Contracts.Services;
using SPbSTU.OPD.ASAP.Core.Presentation;
using User = SPbSTU.OPD.ASAP.Core.Domain.Models.User;
using Role = SPbSTU.OPD.ASAP.Core.Domain.Enums.Role;
using UserGrpc = SPbSTU.OPD.ASAP.Core.Presentation.User;
using RoleGrpc = SPbSTU.OPD.ASAP.Core.Presentation.Role;

namespace SPbSTU.OPD.ASAP.Core.Services;

public class UsersGrpcService(IUsersService usersService) : UsersService.UsersServiceBase
{
    private readonly IUsersService _usersService = usersService;

    public override async Task<CreateUserResponse> CreateUser(CreateUserRequest request, ServerCallContext context)
    {
        var user = MapToDomain(request.User);
        var id = await _usersService.Create(user, context.CancellationToken);

        return new CreateUserResponse { UserId = id };
    }

    public override async Task<GetUserResponse> GetUser(GetUserRequest request, ServerCallContext context)
    {
        var result = await _usersService.GetByLogin(request.Login, context.CancellationToken);
        if (result is null)
            return new GetUserResponse { IsFound = false };

        var user = MapToGrpc(result);
        return new GetUserResponse { IsFound = true, UserId = result.Id, User = user };
    }

    private static User MapToDomain(UserGrpc user)
    {
        return new User(0, user.Name, user.Login, user.Password, user.Email, (Role)(int)user.Role, user.GithubUsername);
    }

    private static UserGrpc MapToGrpc(User user)
    {
        return new UserGrpc
        {
            Name = user.Name, Login = user.Login, Password = user.Password, Email = user.Email,
            Role = (RoleGrpc)(int)user.Role, GithubUsername = user.GithubUsername
        };
    }
}