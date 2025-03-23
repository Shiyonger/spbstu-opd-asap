using System.Text.Json.Serialization;

namespace SPbSTU.OPD.ASAP.Google.Infrastructure.Contracts;

public sealed class PointsKafka
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("points")]
    public int Points { get; set; }

    [JsonPropertyName("date")]
    public DateTime Date { get; set; }

    [JsonPropertyName("student_position")]
    public Position StudentPosition { get; set; } = null!;

    [JsonPropertyName("assignment_position")]
    public Position AssignmentPosition { get; set; } = null!;

    public sealed class Position
    {
        [JsonPropertyName("cell")]
        public string Cell { get; set; } = null!;

        [JsonPropertyName("spreadsheet_id")]
        public long SpreadSheetId { get; set; }
    }
}