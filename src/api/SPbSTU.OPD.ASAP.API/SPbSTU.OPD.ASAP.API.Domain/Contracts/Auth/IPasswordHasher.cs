namespace SPbSTU.OPD.ASAP.API.Domain.Contracts.Auth;

public interface IPasswordHasher
{
    string Generate(string password);
    
    bool Verify(string providedPassword, string hashedPassword);
}