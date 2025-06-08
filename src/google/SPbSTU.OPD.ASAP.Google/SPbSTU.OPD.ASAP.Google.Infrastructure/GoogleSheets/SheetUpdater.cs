using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using SPbSTU.OPD.ASAP.Google.Domain.Entities;
using SPbSTU.OPD.ASAP.Google.Domain.Interfaces;
using SPbSTU.OPD.ASAP.Google.Infrastructure.Contracts;

namespace SPbSTU.OPD.ASAP.Google.Infrastructure.GoogleSheets;

public class SheetUpdater(IGoogleClientFactory clientFactory) : ISheetUpdater
{
    private readonly SheetsService _sheetsService = clientFactory.CreateSheetsService();
    private readonly object _pointsLockObject = new object();
    private readonly object _queueLockObject = new object();

    public void UpdatePoints(PointsMessage message)
    {
        lock (_pointsLockObject)
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

            request.Execute();
        }
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

    public void UpdateQueue(QueueMessage message)
    {
        lock (_queueLockObject)
        {
            var spreadsheetId = message.SpreadSheetId;
            const string sheetName = "Sheet1";
            const string range = $"{sheetName}!A2:D"; 
        
            var getRequest = _sheetsService.Spreadsheets.Values.Get(spreadsheetId, range);
            var response = getRequest.Execute();
        
            var values = (response.Values ?? new List<IList<object>>()).ToList();
            var studentIndex = values.FindIndex(row => row.Count >= 1 && row[0]?.ToString() == message.StudentName);
            Console.WriteLine(studentIndex + " " + message.StudentName);

            switch (message.Action)
            {
                case "Create":
                    values.Add(CreateRow(message));
                    break;

                case "Update":
                    if (studentIndex == -1) return;
                    var studentRow = values[studentIndex];
                    values.RemoveAt(studentIndex);
                    values.Add(studentRow);
                    break;

                case "Delete":
                    if (studentIndex == -1) return;
                    values.RemoveAt(studentIndex);
                    break;
            }
        
            // var valueRange = new ValueRange { Values = values };
            // var updateRequest = _sheetsService.Spreadsheets.Values.Update(valueRange, spreadsheetId, range);
            // updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
            // updateRequest.Execute();
            
            var clearRequest = _sheetsService.Spreadsheets.Values.Clear(
                new ClearValuesRequest(),
                spreadsheetId,
                range
            );
            clearRequest.Execute();

            if (values.Count > 0)
            {
                var valueRange = new ValueRange { Values = values };
                var updateRequest = _sheetsService.Spreadsheets.Values.Update(
                    valueRange,
                    spreadsheetId,
                    range
                );
                updateRequest.ValueInputOption =
                    SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
                updateRequest.Execute();
            }
        }
    }

    private static IList<object> CreateRow(QueueMessage message)
    {
        return new List<object>
        {
            message.StudentName,
            message.GroupId.ToString(),
            message.SubmissionDate.ToString("yyyy-MM-dd HH:mm:ss"),
            message.GithubLink
        };
    }
}