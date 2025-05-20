using SPbSTU.OPD.ASAP.API.Domain.Models;

namespace SPbSTU.OPD.ASAP.API.Domain.Contracts.Auth;

public interface IJwtProvider
{
    string GenerateToken(User user);
}