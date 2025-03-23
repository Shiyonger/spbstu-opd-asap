using System.Text.Json;
using Confluent.Kafka;

namespace SPbSTU.OPD.ASAP.Google.Infrastructure.Kafka;

public sealed class SystemTextJsonSerializer<T>(JsonSerializerOptions? jsonSerializerOptions = null) : IDeserializer<T>, ISerializer<T>
{
    public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
    {
        return isNull
            ? throw new ArgumentNullException($"Null data encountered deserializing {typeof(T).Name} value.")
            : JsonSerializer.Deserialize<T>(data, jsonSerializerOptions)!;
    }

    public byte[] Serialize(T data, SerializationContext context)
    {
        return JsonSerializer.SerializeToUtf8Bytes(data, jsonSerializerOptions);
    }
}