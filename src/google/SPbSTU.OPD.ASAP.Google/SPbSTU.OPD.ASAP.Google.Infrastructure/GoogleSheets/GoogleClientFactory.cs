using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Microsoft.Extensions.Options;
using SPbSTU.OPD.ASAP.Google.Domain.Interfaces;
using SPbSTU.OPD.ASAP.Google.Infrastructure.Settings;

namespace SPbSTU.OPD.ASAP.Google.Infrastructure.GoogleSheets;

public class GoogleClientFactory: IGoogleClientFactory
{
    
    private readonly GoogleOptions _options;

    public GoogleClientFactory(IOptions<GoogleOptions> options)
    {
        _options = options.Value;
    }

    public SheetsService CreateSheetsService()
    {
        var credential = GoogleCredential.FromFile(_options.CredentialsPath)
            .CreateScoped(SheetsService.Scope.Spreadsheets);

        return new SheetsService(new BaseClientService.Initializer
        {
            HttpClientInitializer = credential,
            ApplicationName = _options.ApplicationName
        });
    }

    public DriveService CreateDriveService()
    {
        var credential = GoogleCredential.FromFile(_options.CredentialsPath)
            .CreateScoped(DriveService.Scope.Drive);

        return new DriveService(new BaseClientService.Initializer
        {
            HttpClientInitializer = credential,
            ApplicationName = _options.ApplicationName
        });
    }
}