using Google.Apis.Drive.v3;
using Google.Apis.Sheets.v4;

namespace SPbSTU.OPD.ASAP.Google.Domain.Interfaces;

public interface IGoogleClientFactory
{
    SheetsService CreateSheetsService();
    DriveService CreateDriveService();
}