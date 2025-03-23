namespace SPbSTU.OPD.ASAP.Google.Infrastructure.Settings;

public record KafkaOptions()
{
    public const string Points = "Points";
    public const string Queue = "Queue";
    
    public required string BootstrapServers { get; init; } = string.Empty;
    public required string GroupId { get; init; } = string.Empty;
    public required string Topic { get; init; } = string.Empty;
    public required int ChannelCapacity { get; init; }
    public required int BufferDelaySeconds { get; init; }
}