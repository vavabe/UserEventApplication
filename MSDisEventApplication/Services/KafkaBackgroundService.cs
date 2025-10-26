using Confluent.Kafka;
using Microsoft.Extensions.Options;
using MSDisEventApplication.Models;
using MSDisEventApplication.Options;
using MSDisEventApplication.Services.Interfaces;
using Newtonsoft.Json;

namespace MSDisEventApplication.Services;

public class KafkaBackgroundService : IHostedService
{
    private readonly ILogger<KafkaBackgroundService> _logger;
    private readonly IEventObservable<UserEvent> _eventObservable;
    private readonly KafkaOptions _kafkaOptions;

    public KafkaBackgroundService(ILogger<KafkaBackgroundService> logger, 
        IEventObservable<UserEvent> eventObservable,
        IOptions<KafkaOptions> kafkaOption)
    {
        _logger = logger;
        _eventObservable = eventObservable;
        _kafkaOptions = kafkaOption.Value;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
       var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = _kafkaOptions.BootstrapServers,
            GroupId = _kafkaOptions.GroupId,
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        using var consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();
        consumer.Subscribe(Environment.GetEnvironmentVariable("KAFKA_TOPIC"));

        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var result = consumer.Consume(cancellationToken);
                    var userEvent = JsonConvert.DeserializeObject<UserEvent>(result.Message.Value);
                    if (userEvent != null)
                        _eventObservable.PublishEvent(userEvent);
                    else
                    {
                        _logger.LogWarning($"Не удалось десериализовать сообщения: {result.Message}");
                        //TODO: send to dlq
                    }
                }
                catch (ConsumeException ex)
                {
                    _logger.LogError($"Ошибка получения сообщения: {ex.Error.Reason}");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Ошибка при получении сообщений");
                }

            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Kafka consumer остановлен.");
        }
        finally
        {
            consumer.Close();
        }
    }
}