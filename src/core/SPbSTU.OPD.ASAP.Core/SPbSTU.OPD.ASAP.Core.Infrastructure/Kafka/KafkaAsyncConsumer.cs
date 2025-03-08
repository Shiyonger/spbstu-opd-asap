using System.Threading.Channels;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Polly;
using SPbSTU.OPD.ASAP.Core.Domain.Common;
using SPbSTU.OPD.ASAP.Core.Infrastructure.Settings;

namespace SPbSTU.OPD.ASAP.Core.Infrastructure.Kafka;

public sealed class KafkaAsyncConsumer<TKey, TValue> : IDisposable
{
    private readonly int _channelCapacity;
    private readonly TimeSpan _bufferDelay;

    private readonly Channel<ConsumeResult<TKey, TValue>> _channel;
    private readonly IConsumer<TKey, TValue> _consumer;
    private readonly IHandler<TKey, TValue> _handler;

    private readonly ILogger<KafkaAsyncConsumer<TKey, TValue>> _logger;

    public KafkaAsyncConsumer(
        IHandler<TKey, TValue> handler,
        KafkaOptions options,
        IDeserializer<TKey>? keyDeserializer,
        IDeserializer<TValue>? valueDeserializer,
        ILogger<KafkaAsyncConsumer<TKey, TValue>> logger)
    {
        var builder = new ConsumerBuilder<TKey, TValue>(
            new ConsumerConfig
            {
                BootstrapServers = options.BootstrapServers,
                GroupId = options.GroupId,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = true,
                EnableAutoOffsetStore = false
            });

        if (keyDeserializer is not null)
        {
            builder.SetKeyDeserializer(keyDeserializer);
        }

        if (valueDeserializer is not null)
        {
            builder.SetValueDeserializer(valueDeserializer);
        }

        _bufferDelay = TimeSpan.FromSeconds(options.BufferDelaySeconds);
        _channelCapacity = options.ChannelCapacity;

        _handler = handler;
        _logger = logger;

        _channel = Channel.CreateBounded<ConsumeResult<TKey, TValue>>(
            new BoundedChannelOptions(_channelCapacity)
            {
                SingleWriter = true,
                SingleReader = true,
                AllowSynchronousContinuations = true,
                FullMode = BoundedChannelFullMode.Wait
            });

        _consumer = builder.Build();
        _consumer.Subscribe(options.Topic);
    }

    public Task Consume(CancellationToken token)
    {
        var handle = HandleCore(token);
        var consume = ConsumeCore(token);

        return Task.WhenAll(handle, consume);
    }

    private async Task HandleCore(CancellationToken token)
    {
        await Task.Yield();

        await foreach (var consumeResults in _channel.Reader
                           .ReadAllAsync(token)
                           .Buffer(_channelCapacity, _bufferDelay)
                           .WithCancellation(token))
        {
            token.ThrowIfCancellationRequested();

            while (true)
            {
                var retryPolicy = Policy
                    .Handle<Exception>()
                    .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromMilliseconds(20));

                try
                {
                    await _handler.Handle(consumeResults, token);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unhandled exception occured");
                    await retryPolicy.ExecuteAsync(() => _handler.Handle(consumeResults, token));
                    continue;
                }

                var partitionLastOffsets = consumeResults
                    .GroupBy(
                        r => r.Partition.Value,
                        (_, f) => f.MaxBy(p => p.Offset.Value));

                foreach (var partitionLastOffset in partitionLastOffsets)
                {
                    _consumer.StoreOffset(partitionLastOffset);
                }

                break;
            }
        }
    }

    private async Task ConsumeCore(CancellationToken token)
    {
        await Task.Yield();

        while (_consumer.Consume(token) is { } result)
        {
            await _channel.Writer.WriteAsync(result, token);
            _logger.LogTrace(
                "{Partition}:{Offset}:WriteToChannel",
                result.Partition.Value,
                result.Offset.Value);
        }

        _channel.Writer.Complete();
    }

    public void Dispose() => _consumer.Close();
}
