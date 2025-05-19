using System.Text.Json.Serialization;

namespace SPbSTU.OPD.ASAP.Core.Infrastructure.Contracts;

public sealed class QueueKafka
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("link")]
    public string Link { get; set; } = null!;

    [JsonPropertyName("student_id")]
    public long StudentId { get; set; }

    [JsonPropertyName("student_name")]
    public string StudentName { get; set; } = null!;

    [JsonPropertyName("group_id")]
    public long GroupId { get; set; }

    [JsonPropertyName("spreadsheet_id")]
    public string SpreadsheetId { get; set; } = null!;

    [JsonPropertyName("submission_date")]
    public DateTime SubmissionDate { get; set; }

    [JsonPropertyName("action")]
    public ActionType Action { get; set; }

    public enum ActionType
    {
        Create,
        Update,
        Delete
    }
}