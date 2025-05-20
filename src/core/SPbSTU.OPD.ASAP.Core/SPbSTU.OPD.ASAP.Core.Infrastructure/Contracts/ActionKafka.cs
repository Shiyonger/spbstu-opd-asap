using System.Text.Json.Serialization;

namespace SPbSTU.OPD.ASAP.Core.Infrastructure.Contracts;

public sealed class ActionKafka
{
    [JsonPropertyName("username")] 
    public string Username { get; set; } = null!;

    [JsonPropertyName("date")]
    public DateTime Date { get; set; }

    [JsonPropertyName("assignment_title")]
    public string AssignmentTitle { get; set; } = null!;
    
    [JsonPropertyName("action")]
    public ActionType Action { get; set; }

    public enum ActionType
    {
        Create,
        Update,
        Delete
    }
}