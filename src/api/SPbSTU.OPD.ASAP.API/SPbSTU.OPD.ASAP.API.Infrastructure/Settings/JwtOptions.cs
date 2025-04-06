namespace SPbSTU.OPD.ASAP.API.Infrastucture.Settings;

public record JwtOptions
{
    public required string SecretKey { get; init; }
    public required int ExpiresHours { get; init; }
}