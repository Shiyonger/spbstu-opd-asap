using SPbSTU.OPD.ASAP.Core.Domain.Models.User;

namespace SPbSTU.OPD.ASAP.Core.Domain.Contracts.Repositories;

public interface IUsersRepository
{
    Task<long> Create(User user, CancellationToken ct);
    
    Task<User?> GetById(long userId, CancellationToken ct);

    Task<User?> GetByLogin(string login, CancellationToken ct);
}