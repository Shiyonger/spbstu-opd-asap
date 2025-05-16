namespace SPbSTU.OPD.ASAP.Google.Infrastructure.Settings;

public class GoogleOptions
{
    public const string SectionName = "Google";

    public string CredentialsPath { get; set; } = null!;
    public string ApplicationName { get; set; } = null!;
}