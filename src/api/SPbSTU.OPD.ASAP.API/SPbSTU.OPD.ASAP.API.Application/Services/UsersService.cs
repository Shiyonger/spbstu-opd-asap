using SPbSTU.OPD.ASAP.API.Domain.Contracts;
using SPbSTU.OPD.ASAP.API.Domain.Enums;
using SPbSTU.OPD.ASAP.API.Domain.Models;

namespace SPbSTU.OPD.ASAP.API.Application.Services;

public class UsersService(IPasswordHasher passwordHasher, IUsersGrpcService usersGrpcService, IJwtProvider jwtProvider) : IUsersService
{
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly IUsersGrpcService _usersGrpcService = usersGrpcService;
    private readonly IJwtProvider _jwtProvider = jwtProvider;

    public async Task Register(string name, string login, string password, string email, string role, string githubLink, CancellationToken ct)
    {
        var hashedPassword = _passwordHasher.Generate(password);
        var user = new User(0, name, login, hashedPassword, email, Enum.Parse<Role>(role), githubLink);

        await _usersGrpcService.CreateUser(user, ct);
    }

    public async Task<AuthResult> Login(string login, string password, CancellationToken ct)
    {
        var result = await _usersGrpcService.GetUser(login, ct);

        if (!result.IsSuccessful)
            return new AuthResult{IsSuccessful = false, ErrorMessage = result.ErrorMessage};

        if (!_passwordHasher.Verify(password, result.User.Password))
            return new AuthResult{IsSuccessful = false, ErrorMessage = "Wrong password"};

        var token = _jwtProvider.GenerateToken(result.User);

        return new AuthResult { IsSuccessful = true, Token = token };
    }
}