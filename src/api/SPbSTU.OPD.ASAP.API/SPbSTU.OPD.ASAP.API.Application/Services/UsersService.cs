using SPbSTU.OPD.ASAP.API.Domain.Contracts;
using SPbSTU.OPD.ASAP.API.Domain.Enums;
using SPbSTU.OPD.ASAP.API.Domain.Models;

namespace SPbSTU.OPD.ASAP.API.Application.Services;

public class UsersService(IPasswordHasher passwordHasher, IUsersGrpcService usersGrpcService, IJwtProvider jwtProvider) : IUsersService
{
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly IUsersGrpcService _usersGrpcService = usersGrpcService;
    private readonly IJwtProvider _jwtProvider = jwtProvider;

    public async Task Register(string name, string login, string password, string email, string role, string githubLink)
    {
        var hashedPassword = _passwordHasher.Generate(password);
        var user = new User(name, login, hashedPassword, email, Enum.Parse<Role>(role), githubLink);

        await _usersGrpcService.CreateUser(user);
    }

    public async Task<string> Login(string login, string password)
    {
        var user = await _usersGrpcService.GetUser(login);

        if (!_passwordHasher.Verify(password, user.Password))
            throw new Exception("Failed to login");

        var token = _jwtProvider.GenerateToken(user);

        return token;
    }
}