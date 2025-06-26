using System.Net.Sockets;
using System.Text;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;
using Wealth.CurrencyManagement.Application.Abstractions;
using Wealth.CurrencyManagement.Domain.Utilities;

namespace Wealth.CurrencyManagement.Infrastructure.EventBus;

public sealed class KafkaEventBus(
    ILogger<KafkaEventBus> logger,
    IServiceProvider serviceProvider,
    IOptions<EventBusOptions> options,
    IJsonSerializer jsonSerializer,
    IOptions<EventBusSubscriptionInfo> subscriptionOptions) : IEventBus, IDisposable, IHostedService
{
    private readonly ResiliencePipeline pipeline = CreateResiliencePipeline(options.Value.RetryCount);
    private readonly string topicName = options.Value.SubscriptionClientName;
    private readonly EventBusSubscriptionInfo subscriptionInfo = subscriptionOptions.Value;
    private IProducer<string, byte[]> producer;
    private IConsumer<string, byte[]> consumer;
    private CancellationTokenSource consumerCts;

    public async Task Publish(IntegrationEvent @event, CancellationToken token = default)
    {
        var eventName = @event.GetType().Name;

        if (logger.IsEnabled(LogLevel.Trace))
        {
            logger.LogTrace("Creating Kafka producer to publish event: {EventId} ({EventName})", @event.Id, eventName);
        }

        if (producer == null)
        {
            throw new InvalidOperationException("Kafka producer is not initialized");
        }

        var body = SerializeMessage(@event);

        await pipeline.ExecuteAsync(async (producer, token) =>
        {
            if (logger.IsEnabled(LogLevel.Trace))
            {
                logger.LogTrace("Publishing event to Kafka: {EventId}", @event.Id);
            }

            var message = new Message<string, byte[]>
            {
                Key = eventName,
                Value = body,
                Timestamp = new Timestamp(Clock.Now)
            };

            var deliveryReport = await producer.ProduceAsync(topicName, message, token);
            
            if (deliveryReport.Status == PersistenceStatus.NotPersisted)
            {
                throw new InvalidOperationException($"Failed to publish event {eventName} to Kafka");
            }
        }, producer, token);
    }

    public void Dispose()
    {
        consumerCts?.Cancel();
        consumer?.Close();
        consumer?.Dispose();
        producer?.Dispose();
    }

    private async Task OnMessageReceived(ConsumeResult<string, byte[]> message)
    {
        var eventName = message.Message.Key;
        var messageBody = message.Message.Value;

        try
        {
            await ProcessEvent(eventName, messageBody);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error Processing message \"{Message}\"", Encoding.UTF8.GetString(messageBody));
            // In Kafka, we don't have a Nack like RabbitMQ. Instead, we can:
            // 1. Log the error and continue (losing the message)
            // 2. Implement a dead-letter queue pattern
            // 3. Seek back to retry (but be careful with infinite loops)
            return;
        }

        // Commit the offset to mark message as processed
        try
        {
            consumer.Commit(message);
        }
        catch (KafkaException ex)
        {
            logger.LogError(ex, "Error committing Kafka message offset");
        }
    }

    private async Task ProcessEvent(string eventName, byte[] message)
    {
        if (logger.IsEnabled(LogLevel.Trace))
        {
            logger.LogTrace("Processing Kafka event: {EventName}", eventName);
        }

        await using var scope = serviceProvider.CreateAsyncScope();

        if (!subscriptionInfo.EventTypes.TryGetValue(eventName, out var eventType))
        {
            logger.LogWarning("Unable to resolve event type for event name {EventName}", eventName);
            return;
        }

        // Deserialize the event
        var integrationEvent = DeserializeMessage(Encoding.UTF8.GetString(message), eventType);

        // REVIEW: This could be done in parallel
        foreach (var handler in scope.ServiceProvider.GetKeyedServices<IIntegrationEventHandler>(eventType))
        {
            if (integrationEvent == null)
                throw new InvalidOperationException($"Unable to resolve integration event {eventType}");

            await handler.Handle(integrationEvent);
        }
    }

    private IntegrationEvent? DeserializeMessage(string message, Type eventType)
    {
        return jsonSerializer.Deserialize(message, eventType) as IntegrationEvent;
    }

    private byte[] SerializeMessage(IntegrationEvent @event)
    {
        return jsonSerializer.SerializeToUtf8Bytes(@event);
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _ = Task.Factory.StartNew(async () =>
            {
                try
                {
                    logger.LogInformation("Starting Kafka connection on a background thread");

                    var config = new ProducerConfig
                    {
                        BootstrapServers = "localhost:9092", // Should come from config
                        EnableIdempotence = true,
                        MessageSendMaxRetries = 3,
                        Acks = Acks.All
                    };

                    producer = new ProducerBuilder<string, byte[]>(config)
                        .SetErrorHandler((_, e) => logger.LogError($"Kafka Producer Error: {e.Reason}"))
                        .Build();

                    var consumerConfig = new ConsumerConfig
                    {
                        BootstrapServers = "localhost:9092", // Should come from config
                        GroupId = topicName,
                        AutoOffsetReset = AutoOffsetReset.Earliest,
                        EnableAutoCommit = false
                    };

                    consumer = new ConsumerBuilder<string, byte[]>(consumerConfig)
                        .SetErrorHandler((_, e) => logger.LogError($"Kafka Consumer Error: {e.Reason}"))
                        .Build();

                    consumer.Subscribe(topicName);

                    consumerCts = new CancellationTokenSource();

                    while (!consumerCts.IsCancellationRequested)
                    {
                        try
                        {
                            var consumeResult = consumer.Consume(consumerCts.Token);
                            await OnMessageReceived(consumeResult);
                        }
                        catch (ConsumeException ex)
                        {
                            logger.LogError(ex, "Error consuming Kafka message");
                        }
                        catch (OperationCanceledException)
                        {
                            // Expected during shutdown
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error starting Kafka connection");
                }
            },
            TaskCreationOptions.LongRunning);

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        consumerCts?.Cancel();
        return Task.CompletedTask;
    }

    private static ResiliencePipeline CreateResiliencePipeline(int retryCount)
    {
        var retryOptions = new RetryStrategyOptions
        {
            ShouldHandle = new PredicateBuilder().Handle<KafkaException>().Handle<SocketException>(),
            MaxRetryAttempts = retryCount,
            DelayGenerator = (context) => ValueTask.FromResult(GenerateDelay(context.AttemptNumber))
        };

        return new ResiliencePipelineBuilder()
            .AddRetry(retryOptions)
            .Build();

        static TimeSpan? GenerateDelay(int attempt)
        {
            return TimeSpan.FromSeconds(Math.Pow(2, attempt));
        }
    }
}