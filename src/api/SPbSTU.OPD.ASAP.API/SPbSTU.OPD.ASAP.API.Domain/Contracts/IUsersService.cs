using SPbSTU.OPD.ASAP.API.Domain.Enums;

namespace SPbSTU.OPD.ASAP.API.Domain.Contracts;

public interface IUsersService
{
    Task Register(string name, string login, string password, string email, string role, string githubLink);
    
    Task<string> Login(string login, string password);
}