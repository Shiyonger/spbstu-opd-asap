namespace SPbSTU.OPD.ASAP.API.Domain.Contracts;

public interface IPasswordHasher
{
    string Generate(string password);
    
    bool Verify(string providedPassword, string hashedPassword);
}