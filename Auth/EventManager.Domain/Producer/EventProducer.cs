using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Confluent.Kafka;

namespace EventManager.Domain.Producer;

internal class EventProducer : IEventProducer
{
    private readonly IProducer<string, string> _producer;
    private readonly JsonSerializerOptions _serializerOptions;
    
    public EventProducer(string configuration)
    {
        var producerConfig = new ProducerConfig
        {
            BootstrapServers = configuration
        };
        _serializerOptions = new JsonSerializerOptions{
            Converters ={
                new JsonStringEnumConverter()
            }
        };

        _producer = new ProducerBuilder<string, string>(producerConfig)
            .Build();    
    }
    
    public async Task Produce<T>(string topic, string key, T producedEvent)
    {
        var serialized = JsonSerializer.Serialize(producedEvent, _serializerOptions);
        var message = new Message<string, string> { Value = serialized, Key = key};

        await _producer.ProduceAsync(topic, message, CancellationToken.None);
            
        Console.WriteLine($"Produced event to topic {topic}: message key = {message.Key} value = {serialized}");
        _producer.Flush(TimeSpan.FromSeconds(10));
        
    }
}