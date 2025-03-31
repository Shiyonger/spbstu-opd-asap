using SPbSTU.OPD.ASAP.API.Domain.Models;

namespace SPbSTU.OPD.ASAP.API.Domain.Contracts;

public interface IUsersGrpcService
{
    Task<long> CreateUser(User user);
    
    Task<User> GetUser(string login);
}