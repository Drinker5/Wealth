using System.Net.Sockets;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using Wealth.CurrencyManagement.Application.Abstractions;

namespace Wealth.CurrencyManagement.Infrastructure.EventBus;

public sealed class RabbitMQEventBus(
    ILogger<RabbitMQEventBus> logger,
    IServiceProvider serviceProvider,
    IOptions<EventBusOptions> options,
    IJsonSerializer jsonSerializer,
    IOptions<EventBusSubscriptionInfo> subscriptionOptions) : IEventBus, IDisposable, IHostedService
{
    private const string ExchangeName = "wealth_event_bus";

    private readonly ResiliencePipeline pipeline = CreateResiliencePipeline(options.Value.RetryCount);
    private readonly string queueName = options.Value.SubscriptionClientName;
    private readonly EventBusSubscriptionInfo subscriptionInfo = subscriptionOptions.Value;
    private IConnection rabbitMQConnection;
    private IChannel consumerChannel;

    public async Task Publish(IntegrationEvent @event, CancellationToken token = default)
    {
        var routingKey = @event.GetType().Name;

        if (logger.IsEnabled(LogLevel.Trace))
        {
            logger.LogTrace("Creating RabbitMQ channel to publish event: {EventId} ({EventName})", @event.Id, routingKey);
        }

        if (rabbitMQConnection == null)
        {
            throw new InvalidOperationException("RabbitMQ connection is not open");
        }

        await using var channel = await rabbitMQConnection.CreateChannelAsync(cancellationToken: token);

        if (logger.IsEnabled(LogLevel.Trace))
        {
            logger.LogTrace("Declaring RabbitMQ exchange to publish event: {EventId}", @event.Id);
        }

        await channel.ExchangeDeclareAsync(exchange: ExchangeName, type: "direct", cancellationToken: token);

        var body = SerializeMessage(@event);

        await pipeline.ExecuteAsync(async (channel, token) =>
        {
            var properties = new BasicProperties
            {
                DeliveryMode = DeliveryModes.Persistent
            };

            if (logger.IsEnabled(LogLevel.Trace))
            {
                logger.LogTrace("Publishing event to RabbitMQ: {EventId}", @event.Id);
            }

            await channel.BasicPublishAsync(
                exchange: ExchangeName,
                routingKey: routingKey,
                mandatory: true,
                basicProperties: properties,
                body: body,
                cancellationToken: token);
        }, channel, token);
    }

    public void Dispose()
    {
        consumerChannel?.Dispose();
    }

    private async Task OnMessageReceived(object sender, BasicDeliverEventArgs eventArgs)
    {
        var eventName = eventArgs.RoutingKey;
        var message = Encoding.UTF8.GetString(eventArgs.Body.Span);

        try
        {
            await ProcessEvent(eventName, message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error Processing message \"{Message}\"", message);
            await consumerChannel.BasicNackAsync(eventArgs.DeliveryTag, multiple: false, requeue: false);
            return;
        }

        // Even on exception we take the message off the queue.
        // in a REAL WORLD app this should be handled with a Dead Letter Exchange (DLX). 
        // For more information see: https://www.rabbitmq.com/dlx.html
        await consumerChannel.BasicAckAsync(eventArgs.DeliveryTag, multiple: false);
    }

    private async Task ProcessEvent(string eventName, string message)
    {
        if (logger.IsEnabled(LogLevel.Trace))
        {
            logger.LogTrace("Processing RabbitMQ event: {EventName}", eventName);
        }

        await using var scope = serviceProvider.CreateAsyncScope();

        if (!subscriptionInfo.EventTypes.TryGetValue(eventName, out var eventType))
        {
            logger.LogWarning("Unable to resolve event type for event name {EventName}", eventName);
            return;
        }

        // Deserialize the event
        var integrationEvent = DeserializeMessage(message, eventType);

        // REVIEW: This could be done in parallel

        // Get all the handlers using the event type as the key
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
        // Messaging is async so we don't need to wait for it to complete. On top of this
        // the APIs are blocking, so we need to run this on a background thread.
        _ = Task.Factory.StartNew(async () =>
            {
                try
                {
                    logger.LogInformation("Starting RabbitMQ connection on a background thread");

                    rabbitMQConnection = serviceProvider.GetRequiredService<IConnection>();
                    if (!rabbitMQConnection.IsOpen)
                    {
                        return;
                    }

                    if (logger.IsEnabled(LogLevel.Trace))
                    {
                        logger.LogTrace("Creating RabbitMQ consumer channel");
                    }

                    consumerChannel = await rabbitMQConnection.CreateChannelAsync(cancellationToken: cancellationToken);
                    consumerChannel.CallbackExceptionAsync += (sender, ea) =>
                    {
                        logger.LogWarning(ea.Exception, "Error with RabbitMQ consumer channel");
                        return Task.CompletedTask;
                    };

                    await consumerChannel.ExchangeDeclareAsync(exchange: ExchangeName,
                        type: "direct",
                        cancellationToken: cancellationToken);

                    await consumerChannel.QueueDeclareAsync(queue: queueName,
                        durable: true,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null,
                        cancellationToken: cancellationToken);

                    if (logger.IsEnabled(LogLevel.Trace))
                    {
                        logger.LogTrace("Starting RabbitMQ basic consume");
                    }

                    var consumer = new AsyncEventingBasicConsumer(consumerChannel);

                    consumer.ReceivedAsync += OnMessageReceived;

                    await consumerChannel.BasicConsumeAsync(
                        queue: queueName,
                        autoAck: false,
                        consumer: consumer,
                        cancellationToken: cancellationToken);

                    foreach (var (eventName, _) in subscriptionInfo.EventTypes)
                    {
                        await consumerChannel.QueueBindAsync(
                            queue: queueName,
                            exchange: ExchangeName,
                            routingKey: eventName,
                            cancellationToken: cancellationToken);
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error starting RabbitMQ connection");
                }
            },
            TaskCreationOptions.LongRunning);

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private static ResiliencePipeline CreateResiliencePipeline(int retryCount)
    {
        // See https://www.pollydocs.org/strategies/retry.html
        var retryOptions = new RetryStrategyOptions
        {
            ShouldHandle = new PredicateBuilder().Handle<BrokerUnreachableException>().Handle<SocketException>(),
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