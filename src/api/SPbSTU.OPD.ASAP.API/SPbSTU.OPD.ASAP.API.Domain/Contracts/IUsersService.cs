using SPbSTU.OPD.ASAP.API.Domain.Enums;
using SPbSTU.OPD.ASAP.API.Domain.Models;

namespace SPbSTU.OPD.ASAP.API.Domain.Contracts;

public interface IUsersService
{
    Task Register(string name, string login, string password, string email, string role, string githubLink, CancellationToken ct);
    
    Task<AuthResult> Login(string login, string password, CancellationToken ct);
}