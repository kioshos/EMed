using System.Text.Json;
using Confluent.Kafka;
using EMed.Application.Interfaces.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace EMed.Infrastructure.Postgres.Kafka;

public sealed class KafkaProducer : IEventProducer, IDisposable
{
    private readonly IProducer<string, string> _producer;
    private readonly ILogger<KafkaProducer> _logger;
    
    public KafkaProducer(IConfiguration configuration, ILogger<KafkaProducer> logger)
    {
        _logger = logger;
        var bootstrapServers = configuration.GetConnectionString("Kafka")
                               ?? throw new InvalidOperationException("Kafka connection string is missing.");

        var config = new ProducerConfig
        {
            BootstrapServers = bootstrapServers,
            Acks = Acks.All,
            EnableIdempotence = true 
        };

        _producer = new ProducerBuilder<string, string>(config).Build();
    }
    
    public async Task PublishAsync<T>(string topic, string key, T message, CancellationToken token = default)
        where T : class
    {
        var payload = JsonSerializer.Serialize(message);
        var kafkaMessage = new Message<string, string>
        {
            Key = key,
            Value = payload
        };

        var result = await _producer.ProduceAsync(topic, kafkaMessage, token);

        _logger.LogInformation(
            "Published to {Topic} [Partition: {Partition}, Offset: {Offset}]",
            result.Topic, result.Partition.Value, result.Offset.Value);
    }

    public void Dispose()
    {
        _producer.Flush(TimeSpan.FromSeconds(10));
        _producer.Dispose();
    }
}