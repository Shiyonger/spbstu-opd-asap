using SPbSTU.OPD.ASAP.API.Domain.Contracts;

namespace SPbSTU.OPD.ASAP.API.Infrastructure.Grpc;

public class UsersGrpcService(UsersService.UsersServiceClient usersClient) : IUsersGrpcService
{
    private readonly UsersService.UsersServiceClient _usersClient = usersClient;

    public async Task<long> CreateUser(Domain.Models.User user)
    {
        var userGrpc = new User
        {
            Name = user.Name, Login = user.Login, Password = user.Password, Email = user.Email,
            Role = (Role)(int)user.Role, GithubLink = user.GithubLink
        };
        
        var response = await _usersClient.CreateUserAsync(new CreateUserRequest { User = userGrpc });
        
        return response.UserId;
    }

    public async Task<Domain.Models.User> GetUser(string login)
    {
        var response = await _usersClient.GetUserAsync(new GetUserRequest { Login = login });
        
        var user = new Domain.Models.User(response.User.Name, response.User.Login, response.User.Password,
            response.User.Email, (Domain.Enums.Role)(int)response.User.Role, response.User.GithubLink);
        
        return user;
    }
}