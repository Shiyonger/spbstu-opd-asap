namespace SPbSTU.OPD.ASAP.Core.Infrastructure.Settings;

public record KafkaPublisherOptions()
{
    public const string Points = "Points";
    public const string Queue = "Queue";
    
    public required string BootstrapServers { get; init; } = string.Empty;
    public required string Topic { get; init; } = string.Empty;
    public required int BatchSize { get; init; }
    public required int LingerMs { get; init; }
    public required int ChunkSize { get; init; }
}