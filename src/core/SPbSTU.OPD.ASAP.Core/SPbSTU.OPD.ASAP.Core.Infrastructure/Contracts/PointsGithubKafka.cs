using System.Text.Json.Serialization;

namespace SPbSTU.OPD.ASAP.Core.Infrastructure.Contracts;

public sealed class PointsGithubKafka
{
    [JsonPropertyName("assignment_title")]
    public string AssignmentTitle { get; set; } = null!;

    [JsonPropertyName("course_title")]
    public string CourseTitle { get; set; } = null!;

    [JsonPropertyName("username")]
    public string Username { get; set; } = null!;

    [JsonPropertyName("date")]
    public DateTime Date { get; set; }

    [JsonPropertyName("points")]
    public int Points { get; set; }
}