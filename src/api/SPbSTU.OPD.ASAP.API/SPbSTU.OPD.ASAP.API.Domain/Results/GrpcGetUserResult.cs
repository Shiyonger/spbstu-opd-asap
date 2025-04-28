using SPbSTU.OPD.ASAP.API.Domain.Models;

namespace SPbSTU.OPD.ASAP.API.Domain.Results;

public class GrpcGetUserResult
{
    public bool IsSuccessful { get; init; }
    public string ErrorMessage { get; init; } = string.Empty;
    public User User { get; init; } = null!;
}