using SPbSTU.OPD.ASAP.Core.Domain.Contracts.Repositories;
using SPbSTU.OPD.ASAP.Core.Domain.Contracts.Services;
using SPbSTU.OPD.ASAP.Core.Domain.Models;

namespace SPbSTU.OPD.ASAP.Core.Application.Services;

public class UsersService(IUsersRepository usersRepository) : IUsersService
{
    private readonly IUsersRepository _usersRepository = usersRepository;

    public Task<long> Create(User user, CancellationToken ct)
    {
        return _usersRepository.Create(user, ct);
    }

    public Task<User?> GetById(long userId, CancellationToken ct)
    {
        return _usersRepository.GetById(userId, ct);
    }

    public Task<User?> GetByLogin(string login, CancellationToken ct)
    {
        return _usersRepository.GetByLogin(login, ct);
    }
}