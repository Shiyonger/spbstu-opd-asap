using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Microsoft.Extensions.Options;
using SPbSTU.OPD.ASAP.Google.Domain.Entities;
using SPbSTU.OPD.ASAP.Google.Domain.Interfaces;
using SPbSTU.OPD.ASAP.Google.Domain.ValueObjects;
using SPbSTU.OPD.ASAP.Google.Infrastructure.Settings;

namespace SPbSTU.OPD.ASAP.Google.Infrastructure.GoogleSheets;

public class SpreadSheetBuilder: ISpreadSheetBuilder
{
    private readonly SheetsService _sheetsService;
    private readonly DriveService _driveService;

    public SpreadSheetBuilder(IGoogleClientFactory clientFactory)
    {
        _sheetsService = clientFactory.CreateSheetsService();
        _driveService = clientFactory.CreateDriveService();
    }
    
    public async Task<CoursePosition> BuildAsync(
        
        Course course,
        CancellationToken cancellationToken = default)
    
    {
        var spreadsheet = await CreateSpreadsheetAsync(course.Title, cancellationToken);

        string spreadsheetId = spreadsheet.SpreadsheetId;
        string spreadsheetLink = spreadsheet.SpreadsheetUrl;

        await MakePublicAsync(spreadsheetId, cancellationToken);
        
        var studentPositions = new List<StudentPosition>();
        var assignmentPositions = new List<AssignmentPosition>();
        
        var students = course.Students;
        var assignments = course.Assignments;

        int studentRow = 2;
        foreach (var student in students)
        {
            string cell = $"A{studentRow++}";
            studentPositions.Add(new StudentPosition(student.Id, student.Name, cell));
        }

        int assignmentCol = 2;
        foreach (var assignment in assignments)
        {
            var queueTitle = $"Очередь проверки: {assignment.Title}";
            var queueSpreadsheet = await CreateSpreadsheetAsync(queueTitle, cancellationToken);
    
            string queueSpreadsheetId = queueSpreadsheet.SpreadsheetId!;
            string queueSpreadsheetLink = $"https://docs.google.com/spreadsheets/d/{queueSpreadsheetId}";
            
            await MakePublicAsync(queueSpreadsheetId, cancellationToken);
            
            await WriteHeaderRowAsync(queueSpreadsheetId, cancellationToken);

            await AutoResizeColumnsAsync(
                queueSpreadsheetId, 
                queueSpreadsheet.Sheets[0].Properties.SheetId ?? 0,   
                1, 
                26, 
                cancellationToken);
            
            string colLetter = ColumnLetter(assignmentCol++);
            string cell = $"{colLetter}1";
            
            assignmentPositions.Add(new AssignmentPosition(assignment.Id, assignment.Title, cell, queueSpreadsheetLink, queueSpreadsheetId));
        }

        var sheetId = spreadsheet.Sheets[0].Properties.SheetId ?? 0; 
        var requests = new List<Request>();
        
        requests.Add(new Request
        {
            UpdateCells = new UpdateCellsRequest
            {
                Start = new GridCoordinate { SheetId = sheetId, RowIndex = 0, ColumnIndex = 0 },
                Rows = new List<RowData>
                {
                    new RowData
                    {
                        Values = new List<CellData>
                        {
                            new CellData
                            {
                                UserEnteredValue = new ExtendedValue { StringValue = "ФИО" },
                                UserEnteredFormat = new CellFormat
                                {
                                    TextFormat = new TextFormat { Bold = true }
                                }
                            }
                        }
                    }
                },
                Fields = "userEnteredValue,userEnteredFormat.textFormat.bold"
            }
        });

        for (int i = 0; i < students.Count; i++)
        {
            requests.Add(new Request
            {
                UpdateCells = new UpdateCellsRequest
                {
                    Start = new GridCoordinate { SheetId = sheetId, RowIndex = i + 1, ColumnIndex = 0 },
                    Rows = new List<RowData>
                    {
                        new RowData
                        {
                            Values = new List<CellData>
                            {
                                new CellData
                                {
                                    UserEnteredValue = new ExtendedValue { StringValue = students[i].Name }
                                }
                            }
                        }
                    },
                    Fields = "userEnteredValue"
                }
            });
        }

        for (int j = 0; j < assignments.Count; j++)
        {
            requests.Add(new Request
            {
                UpdateCells = new UpdateCellsRequest
                {
                    Start = new GridCoordinate { SheetId = sheetId, RowIndex = 0, ColumnIndex = j + 1 },
                    Rows = new List<RowData>
                    {
                        new RowData
                        {
                            Values = new List<CellData>
                            {
                                new CellData
                                {
                                    UserEnteredValue = new ExtendedValue { StringValue = assignments[j].Title },
                                    UserEnteredFormat = new CellFormat
                                    {
                                        TextFormat = new TextFormat { Bold = true }
                                    }
                                }
                            }
                        }
                    },
                    Fields = "userEnteredValue,userEnteredFormat.textFormat.bold"
                }
            });
        }

        var batchUpdateRequest = new BatchUpdateSpreadsheetRequest { Requests = requests };
        await _sheetsService.Spreadsheets.BatchUpdate(batchUpdateRequest, spreadsheetId).ExecuteAsync(cancellationToken);

        
                
        await AutoResizeColumnsAsync(
            spreadsheetId, 
            sheetId,   
            1, 
            26, 
            cancellationToken);
        
        return new CoursePosition(
            course.Id,
            course.Title,
            spreadsheetId,
            spreadsheetLink,
            studentPositions,
            assignmentPositions);
    }
    
    private async Task<Spreadsheet> CreateSpreadsheetAsync(string title, CancellationToken cancellationToken)
    {
        var spreadsheet = new Spreadsheet
        {
            Properties = new SpreadsheetProperties
            {
                Title = title
            }
        };

        var request = _sheetsService.Spreadsheets.Create(spreadsheet);
        
        return await request.ExecuteAsync(cancellationToken);
    }
    
    private string ColumnLetter(int columnNumber)
    {
        string column = string.Empty;
        while (columnNumber > 0)
        {
            int remainder = (columnNumber - 1) % 26;
            column = (char)('A' + remainder) + column;
            columnNumber = (columnNumber - 1) / 26;
        }
        return column;
    }
    
    private async Task WriteHeaderRowAsync(string spreadsheetId, CancellationToken cancellationToken)
    {
        var valueRange = new ValueRange
        {
            Values = new List<IList<object>>
            {
                new List<object> { "ФИО", "Группа", "Дата", "GitHub" }
            }
        };

        var updateRequest = _sheetsService.Spreadsheets.Values.Update(valueRange, spreadsheetId, "A1:D1");
        updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;

        await updateRequest.ExecuteAsync(cancellationToken);
        
        var request = new RepeatCellRequest
        {
            Range = new GridRange
            {
                SheetId = 0,
                StartRowIndex = 0,
                EndRowIndex = 1
            },
            Cell = new CellData
            {
                UserEnteredFormat = new CellFormat
                {
                    TextFormat = new TextFormat { Bold = true }
                }
            },
            Fields = "userEnteredFormat.textFormat.bold"
        };

        var batchUpdate = new BatchUpdateSpreadsheetRequest
        {
            Requests = new List<Request>
            {
                new Request { RepeatCell = request }
            }
        };

        await _sheetsService.Spreadsheets.BatchUpdate(batchUpdate, spreadsheetId).ExecuteAsync(cancellationToken);
    }
    
    private async Task MakePublicAsync(string spreadsheetId, CancellationToken cancellationToken)
    {
        var permission = new Permission
        {
            Type = "anyone",
            Role = "reader"
        };

        await _driveService.Permissions.Create(permission, spreadsheetId)
            .ExecuteAsync(cancellationToken);
    }
    
    public async Task AutoResizeColumnsAsync(string spreadsheetId, int sheetId, int startColumnIndex, int endColumnIndex, CancellationToken cancellationToken)
    {
        var autoResizeRequest = new Request
        {
            AutoResizeDimensions = new AutoResizeDimensionsRequest
            {
                Dimensions = new DimensionRange
                {
                    SheetId = sheetId,
                    Dimension = "COLUMNS",
                    StartIndex = startColumnIndex,
                    EndIndex = endColumnIndex
                }
            }
        };

        var batchUpdateRequest = new BatchUpdateSpreadsheetRequest
        {
            Requests = new List<Request> { autoResizeRequest }
        };

        await _sheetsService.Spreadsheets
            .BatchUpdate(batchUpdateRequest, spreadsheetId)
            .ExecuteAsync(cancellationToken);
    }
}