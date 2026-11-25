namespace EMed.Application.Interfaces.Kafka;

public interface IEventProducer
{
    Task PublishAsync<T>(string topic, string key, T message, CancellationToken cancellationToken = default)
        where T : class;
}