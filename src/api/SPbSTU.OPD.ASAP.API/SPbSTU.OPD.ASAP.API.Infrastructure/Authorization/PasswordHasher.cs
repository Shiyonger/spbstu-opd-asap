using SPbSTU.OPD.ASAP.API.Domain.Contracts;
using SPbSTU.OPD.ASAP.API.Domain.Contracts.Auth;

namespace SPbSTU.OPD.ASAP.API.Infrastructure.Authorization;

public class PasswordHasher : IPasswordHasher
{
    public string Generate(string password)
    {
        // return BCrypt.Net.BCrypt.EnhancedHashPassword(password);
        return password;
    }

    public bool Verify(string providedPassword, string hashedPassword)
    {
        // return BCrypt.Net.BCrypt.EnhancedVerify(providedPassword, hashedPassword);
        return providedPassword == hashedPassword;
    }
}