using SPbSTU.OPD.ASAP.Core.Domain.Models;

namespace SPbSTU.OPD.ASAP.Core.Domain.Contracts.Services;

public interface IUsersService
{
    Task<long> Create(User user, CancellationToken ct);
    
    Task<User?> GetById(long userId, CancellationToken ct);

    Task<User?> GetByLogin(string login, CancellationToken ct);
}