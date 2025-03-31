using SPbSTU.OPD.ASAP.API.Domain.Models;

namespace SPbSTU.OPD.ASAP.API.Domain.Contracts;

public interface IJwtProvider
{
    string GenerateToken(User user);
}