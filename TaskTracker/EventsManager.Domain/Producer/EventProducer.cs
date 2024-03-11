using System.Text;
using Confluent.Kafka;

namespace EventsManager.Domain.Producer;

internal class EventProducer : IEventProducer
{
    private readonly IProducer<string, string> _producer;
    private Queue<(string, Message<string, string>)> _delayedMessages;
    private readonly Thread retryThread;

    private async Task Retry()
    {
        Thread.CurrentThread.IsBackground = true;
        while (_delayedMessages.Count > 0)
        {
            Thread.Sleep(4000);
            var (topic, message) = _delayedMessages.Peek();
            try
            {
                await _producer.ProduceAsync(topic, message, CancellationToken.None);
                _delayedMessages.Dequeue();
            }
            catch (ProduceException<string, string>)
            {
            }
        }
    }
    public EventProducer(string configuration)
    {
        _delayedMessages = new Queue<(string, Message<string, string>)>();
        retryThread = new Thread(async () => await  Retry());
        var producerConfig = new ProducerConfig
        {
            BootstrapServers = configuration
        };

        var builder = new ProducerBuilder<string, string>(producerConfig);
        _producer = builder.Build();
    }

    public async Task Produce(string topic, string version, string producedEvent)
    {
        var headers = new Headers();
        headers.Add( "Version", Encoding.UTF8.GetBytes(version));
        var message = new Message<string, string>
        {
            Value = producedEvent, 
            Key = version, 
            Headers = headers
        };

        try
        {
            await _producer.ProduceAsync(topic, message, CancellationToken.None);
        }
        catch (ProduceException<string, string> exception)
        {
            if (exception.DeliveryResult.Status == PersistenceStatus.NotPersisted)
            {
                _delayedMessages.Enqueue((topic, message));
                if (!retryThread.IsAlive)
                {
                    retryThread.Start();
                }
            }
        }
        Console.WriteLine($"Produced event to topic {topic}: key = {version} value = {producedEvent}");
    }
}