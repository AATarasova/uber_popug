using System.Text;
using System.Text.Json;
using Confluent.Kafka;

namespace EventsManager.Domain;

public class GuidSerializer : IAsyncSerializer<Guid>, IAsyncDeserializer<Guid>, IDeserializer<Guid>
{
    public async Task<byte[]> SerializeAsync(Guid data, SerializationContext context) => 
        await Task.FromResult(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(data)));

    public async Task<Guid> DeserializeAsync(ReadOnlyMemory<byte> data, bool isNull, SerializationContext context) => 
        await Task.FromResult(JsonSerializer.Deserialize<Guid>(Encoding.UTF8.GetString(data.Span)));

    public Guid Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context) =>
        JsonSerializer.Deserialize<Guid>(Encoding.UTF8.GetString(data));
}
