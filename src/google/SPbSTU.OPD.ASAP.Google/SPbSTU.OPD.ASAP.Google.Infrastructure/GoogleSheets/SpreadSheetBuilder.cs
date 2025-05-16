using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Microsoft.Extensions.Options;
using SPbSTU.OPD.ASAP.Google.Domain.Entities;
using SPbSTU.OPD.ASAP.Google.Domain.ValueObjects;
using SPbSTU.OPD.ASAP.Google.Infrastructure.Settings;

namespace SPbSTU.OPD.ASAP.Google.Infrastructure.GoogleSheets;

public class SpreadSheetBuilder
{
    private readonly SheetsService _sheetsService;
    private readonly DriveService _driveService;

    public SpreadSheetBuilder(GoogleClientFactory clientFactory)
    {
        _sheetsService = clientFactory.CreateSheetsService();
        _driveService = clientFactory.CreateDriveService();
    }
    
    public async Task<SpreadSheetResult> BuildAsync(
        
        IReadOnlyList<Student> students,
        IReadOnlyList<Assignment> assignments,
        CancellationToken cancellationToken = default)
    {
        Spreadsheet spreadsheet = new Spreadsheet
        {
            Properties = new SpreadsheetProperties
            {
                Title = assignments[0].Course.Title
            }
        };

        var request = _sheetsService.Spreadsheets.Create(spreadsheet);
        var created = request.Execute();
        string spreadsheetId = created.SpreadsheetId;

        var permission = new Permission
        {
            Type = "anyone",
            Role = "reader"
        };

        var permissionRequest = _driveService.Permissions.Create(permission, spreadsheetId);
        permissionRequest.Fields = "id";
        permissionRequest.Execute();
        
        
        
        
        
    }
    
}