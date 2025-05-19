using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using SPbSTU.OPD.ASAP.Google.Domain.Entities;
using SPbSTU.OPD.ASAP.Google.Domain.Interfaces;
using SPbSTU.OPD.ASAP.Google.Infrastructure.Contracts;

namespace SPbSTU.OPD.ASAP.Google.Infrastructure.GoogleSheets;

public class SheetUpdater(IGoogleClientFactory clientFactory) : ISheetUpdater
{
    private readonly SheetsService _sheetsService = clientFactory.CreateSheetsService();

    public async Task UpdatePointsAsync(PointsMessage message, CancellationToken cancellationToken)
    {
        var cell = GetIntersectionCell(
            message.StudentPosition.Cell,
            message.AssignmentPosition.Cell);

        var range = $"{cell}:{cell}";

        var valueRange = new ValueRange
        {
            Values = new List<IList<object>> { new List<object> { message.Points } }
        };

        var request = _sheetsService.Spreadsheets.Values.Update(
            valueRange,
            message.AssignmentPosition.SpreadsheetId,
            range);

        request.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;

        await request.ExecuteAsync(cancellationToken);
    }

    private string GetIntersectionCell(string studentCell, string assignmentCell)
    {
        var studentRow = ExtractRow(studentCell);
        var assignmentCol = ExtractColumn(assignmentCell);
        return $"{assignmentCol}{studentRow}";
    }

    private static int ExtractRow(string cell) =>
        int.Parse(new string(cell.Where(char.IsDigit).ToArray()));

    private static string ExtractColumn(string cell) =>
        new string(cell.Where(char.IsLetter).ToArray());

    public async Task UpdateQueueAsync(QueueMessage message, CancellationToken cancellationToken)
    {
        
    }
}