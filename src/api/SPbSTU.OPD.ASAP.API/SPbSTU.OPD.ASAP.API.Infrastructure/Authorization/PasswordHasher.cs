using SPbSTU.OPD.ASAP.API.Domain.Contracts;
using SPbSTU.OPD.ASAP.API.Domain.Contracts.Auth;

namespace SPbSTU.OPD.ASAP.API.Infrastructure.Authorization;

public class PasswordHasher : IPasswordHasher
{
    public string Generate(string password) => BCrypt.Net.BCrypt.EnhancedHashPassword(password);

    public bool Verify(string providedPassword, string hashedPassword)
    {
        // BCrypt.Net.BCrypt.EnhancedVerify(providedPassword, hashedPassword);
        return true;
    }
}