namespace SPbSTU.OPD.ASAP.API.Domain.Models;

public class GrpcGetUserResult
{
    public bool IsSuccessful { get; init; }
    public string ErrorMessage { get; init; } = string.Empty;
    public User User { get; init; } = null!;
}