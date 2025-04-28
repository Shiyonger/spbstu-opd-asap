using SPbSTU.OPD.ASAP.API.Domain.Results;

namespace SPbSTU.OPD.ASAP.API.Domain.Contracts.Services;

public interface IUsersService
{
    Task Register(string name, string login, string password, string email, string role, string githubLink, CancellationToken ct);
    
    Task<AuthResult> Login(string login, string password, CancellationToken ct);
}