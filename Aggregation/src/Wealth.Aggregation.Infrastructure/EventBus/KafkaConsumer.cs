using System.Text;
using Confluent.Kafka;
using Google.Protobuf;
using Google.Protobuf.Reflection;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Infrastructure.EventBus;
using Type = System.Type;

namespace Wealth.Aggregation.Infrastructure.EventBus;

public sealed class KafkaConsumer(
    ILogger<KafkaConsumer> logger,
    IServiceProvider serviceProvider,
    IOptions<KafkaConsumerOptions> options,
    IOptions<EventBusSubscriptionInfo> subscriptionOptions) : IDisposable, IHostedService
{
    private IConsumer<string, byte[]> consumer;

    private CancellationTokenSource consumerCts;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _ = Task.Factory.StartNew(async () =>
            {
                try
                {
                    await StartConsumer();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error starting Kafka connection");
                }
            },
            TaskCreationOptions.LongRunning);

        return Task.CompletedTask;
    }

    private async Task StartConsumer()
    {
        logger.LogInformation("Starting Kafka connection on a background thread");

        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = options.Value.BootstrapServers,
            GroupId = options.Value.GroupId,
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = false,
            // EnableAutoOffsetStore = false,
        };

        consumer = new ConsumerBuilder<string, byte[]>(consumerConfig)
            .SetErrorHandler((_, e) => logger.LogError("Kafka Consumer Error: {Reason}", e.Reason))
            .Build();

        consumer.Subscribe(options.Value.Topics);

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

    private async Task OnMessageReceived(ConsumeResult<string, byte[]> message)
    {
        var key = message.Message.Key;
        var messageBody = message.Message.Value;
        var eventTypeHeader = message.Message.Headers.FirstOrDefault(i => i.Key == options.Value.EventTypeHeader);
        if (eventTypeHeader == null)
        {
            logger.LogWarning("Received message without EventType header");
            return;
        }

        try
        {
            var eventName = Encoding.UTF8.GetString(eventTypeHeader.GetValueBytes());
            await ProcessEvent(eventName, messageBody);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error Processing message \"{Message}\"", Encoding.UTF8.GetString(messageBody));
            consumer.Seek(message.TopicPartitionOffset);
            return;
        }

        try
        {
            // Commit the offset to mark message as processed
            // consumer.StoreOffset(message);
            consumer.Commit(message);
        }
        catch (KafkaException ex)
        {
            logger.LogError(ex, "Commit error: {Reason}", ex.Error.Reason);
        }
    }

    private async Task ProcessEvent(string eventName, byte[] message)
    {
        if (logger.IsEnabled(LogLevel.Trace))
        {
            logger.LogTrace("Processing Kafka event: {EventName}", eventName);
        }

        await using var scope = serviceProvider.CreateAsyncScope();

        if (!subscriptionOptions.Value.EventTypes.TryGetValue(eventName, out var eventType))
        {
            logger.LogWarning("Unable to resolve event type for event name {EventName}", eventName);
            return;
        }

        // Deserialize the event
        var integrationEvent = DeserializeMessage(message, eventType);

        // REVIEW: This could be done in parallel
        foreach (var handler in scope.ServiceProvider.GetKeyedServices<IIntegrationEventHandler>(eventType))
        {
            if (integrationEvent == null)
                throw new InvalidOperationException($"Unable to resolve integration event {eventType}");

            await handler.Handle(integrationEvent);
        }
    }

    private IMessage? DeserializeMessage(byte[] message, Type eventType)
    {
        var descriptorProperty = eventType.GetProperty("Descriptor");
        if (descriptorProperty == null)
            throw new InvalidOperationException($"Type {eventType.Name} doesn't have Descriptor property");

        var descriptor = descriptorProperty.GetValue(null) as MessageDescriptor;
        if (descriptor == null)
            throw new InvalidOperationException($"Type {eventType.Name} doesn't have a parser");

        var any = Any.Parser.ParseFrom(message);
        var typeRegistry = TypeRegistry.FromMessages(descriptor);
        return any.Unpack(typeRegistry);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        consumerCts?.Cancel();
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        consumerCts?.Cancel();
        consumer?.Close();
        consumer?.Dispose();
    }
}