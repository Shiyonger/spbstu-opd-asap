using Confluent.Kafka;
using SPbSTU.OPD.ASAP.Core.Infrastructure.Settings;

namespace SPbSTU.OPD.ASAP.Core.Infrastructure.Kafka;

public sealed class KafkaPublisher<TKey, TValue> : IDisposable
{
    private readonly IProducer<TKey, TValue> _producer;
    private readonly string _topic;
    private readonly int _chunkSize;

    public KafkaPublisher(
        KafkaPublisherOptions options,
        ISerializer<TKey>? keySerializer,
        ISerializer<TValue>? valueSerializer)
    {
        _topic = options.Topic;

        var builder = new ProducerBuilder<TKey, TValue>(
            new ProducerConfig
            {
                BootstrapServers = options.BootstrapServers,
                BatchSize = options.BatchSize,
                LingerMs = options.LingerMs,
            });

        if (keySerializer is not null)
        {
            builder.SetKeySerializer(keySerializer);
        }

        if (valueSerializer is not null)
        {
            builder.SetValueSerializer(valueSerializer);
        }
        
        _chunkSize = options.ChunkSize;

        _producer = builder.Build();
    }

    public void Dispose()
    {
        _producer.Flush();
        _producer.Dispose();
    }

    public async Task Publish(IEnumerable<(TKey key, TValue value)> messages, CancellationToken token)
    {
        var tasks = new List<Task>(_chunkSize);
    
        foreach (var messageChunk in messages.Chunk(_chunkSize))
        {
            tasks.Clear();
    
            foreach (var (key, value) in messageChunk)
            {
                tasks.Add(
                    _producer.ProduceAsync(
                        _topic,
                        new Message<TKey, TValue> { Key = key, Value = value },
                        token));
            }
    
            await Task.WhenAll(tasks);
        }
    }
}
