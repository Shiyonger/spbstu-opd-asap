namespace SPbSTU.OPD.ASAP.API.Domain.Results;

public class AuthResult
{
    public bool IsSuccessful { get; init; }
    public string ErrorMessage { get; init; } = string.Empty;
    public string Token { get; init; } = string.Empty;
}