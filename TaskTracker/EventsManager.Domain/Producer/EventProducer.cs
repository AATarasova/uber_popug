using Confluent.Kafka;

namespace EventsManager.Domain.Producer;

internal class EventProducer : IEventProducer
{
    private readonly IProducer<string, string> _producer;

    public EventProducer(string configuration)
    {
        var producerConfig = new ProducerConfig
        {
            BootstrapServers = configuration
        };

        _producer = new ProducerBuilder<string, string>(producerConfig).Build();
    }

    public async Task Produce(string topic, string key, string producedEvent)
    {
        var message = new Message<string, string> { Value = producedEvent, Key = key };

        await _producer.ProduceAsync(topic, message, CancellationToken.None);

        Console.WriteLine($"Produced event to topic {topic}: key = {key} value = {producedEvent}");
    }
}