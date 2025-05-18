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

    [JsonPropertyName("mentor_id")]
    public long MentorId { get; set; }

    [JsonPropertyName("mentor_name")]
    public string MentorName { get; set; } = null!;

    [JsonPropertyName("assignment_id")]
    public long AssignmentId { get; set; }

    [JsonPropertyName("assignment_title")]
    public string AssignmentTitle { get; set; } = null!;

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