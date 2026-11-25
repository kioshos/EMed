using System.Text.Json;
using Confluent.Kafka;
using EMed.Application.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EMed.BackgroundServices;

public class AppointmentLoggingConsumer : BackgroundService
{
    private readonly IConfiguration _configuration;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<AppointmentLoggingConsumer> _logger;
    
    private const string TopicName = "appointment-logged";
    private const string GroupId = "appointment-logged-group";

    public AppointmentLoggingConsumer(IConfiguration configuration, 
        IServiceScopeFactory scopeFactory, ILogger<AppointmentLoggingConsumer> logger)
    {
        _configuration = configuration;
        _scopeFactory = scopeFactory;
        _logger = logger;
    }
    private async Task ConsumeLoop(CancellationToken stoppingToken)
    {
        var bootstrapServers = _configuration.GetConnectionString("Kafka")
                               ?? throw new InvalidOperationException("Kafka connection string is missing.");

        var config = new ConsumerConfig
        {
            BootstrapServers = bootstrapServers,
            GroupId = GroupId,
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = false
        };

        using var consumer = new ConsumerBuilder<string, string>(config).Build();
        consumer.Subscribe(TopicName);

        _logger.LogInformation("ProjectOnboardingConsumer started listening on '{Topic}'", TopicName);

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var result = consumer.Consume(stoppingToken);
                    var @event = JsonSerializer.Deserialize<AppointmentCreatedEvent>(result.Message.Value);

                    if (@event is not null)
                    {
                        await ProcessAppointmentCreationAsync(@event, stoppingToken);
                    }

                    consumer.Commit(result);
                }
                catch (ConsumeException ex)
                {
                    _logger.LogError(ex, "Kafka consume error: {Reason}", ex.Error.Reason);
                    await Task.Delay(2000, stoppingToken);
                }
                catch (Exception ex) when (ex is not OperationCanceledException)
                {
                    _logger.LogError(ex, "Failed to onboard project from Kafka event");
                }
            }
        }
        catch (OperationCanceledException)
        {
            // Коректне завершення при зупинці контейнера
        }
        finally
        {
            consumer.Close();
        }
    }
    private async Task ProcessAppointmentCreationAsync(AppointmentCreatedEvent @event, CancellationToken ct)
    {
        _logger.LogInformation(
            "[ONBOARDING WORKER] Processing new appointment '{Title}' for ({PatientId})...",
            @event.Id, @event.PatientId);
    }
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.Run(() => ConsumeLoop(stoppingToken), stoppingToken);
    }
}