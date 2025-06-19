using System.Diagnostics;
using System.Net.Sockets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using Wealth.CurrencyManagement.Application.Abstractions;
using Wealth.CurrencyManagement.Application.DomainEvents;

namespace Wealth.CurrencyManagement.Infrastructure.EventBus;

public class EventBusModule : IServiceModule
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IEventBus, RabbitMQEventBus>();
        // Start consuming messages as soon as the application starts
        services.AddSingleton<IHostedService>(sp => (RabbitMQEventBus)sp.GetRequiredService<IEventBus>());

        services.Configure<EventBusOptions>(configuration.GetSection(EventBusOptions.Section));
        services.AddSubscription<SomeIntegrationEvent, SomeIntegrationEventHandler>();

        AddRabbitMQClient(services, configuration);
    }

    private void AddRabbitMQClient(IServiceCollection services, IConfiguration configuration)
    {
        var configSection = configuration.GetSection(EventBusOptions.Section);
        var settings = new EventBusOptions();
        configSection.Bind(settings);

        services.AddSingleton<IConnectionFactory>(sp => CreateConnectionFactory(configSection));
        services.AddSingleton<IConnection>(sp =>
            CreateConnection(sp.GetRequiredService<IConnectionFactory>(), settings.RetryCount).AsTask().GetAwaiter().GetResult());
    }

    private IConnectionFactory CreateConnectionFactory(IConfigurationSection configSection)
    {
        var factory = new ConnectionFactory();
        var configurationOptionsSection = configSection.GetSection("ConnectionFactory");
        configurationOptionsSection.Bind(factory);
        return factory;
    }

    private static ValueTask<IConnection> CreateConnection(IConnectionFactory factory, int retryCount, CancellationToken cancellationToken = default)
    {
        var resiliencePipelineBuilder = new ResiliencePipelineBuilder();
        if (retryCount > 0)
        {
            resiliencePipelineBuilder.AddRetry(new RetryStrategyOptions
            {
                ShouldHandle = static args => args.Outcome is { Exception: SocketException or BrokerUnreachableException }
                    ? PredicateResult.True()
                    : PredicateResult.False(),
                BackoffType = DelayBackoffType.Exponential,
                MaxRetryAttempts = retryCount,
                Delay = TimeSpan.FromSeconds(1),
            });
        }

        var resiliencePipeline = resiliencePipelineBuilder.Build();

        return resiliencePipeline.ExecuteAsync(static async (factory, cancellationToken) =>
            await factory.CreateConnectionAsync(cancellationToken), factory, cancellationToken);
    }
}

public static class EventBusModuleExtensions
{
    public static IServiceCollection AddSubscription<T, TH>(this IServiceCollection services)
        where T : IntegrationEvent
        where TH : class, IIntegrationEventHandler<T>
    {
        // Use keyed services to register multiple handlers for the same event type
        // the consumer can use IKeyedServiceProvider.GetKeyedService<IIntegrationEventHandler>(typeof(T)) to get all
        // handlers for the event type.
        services.AddKeyedTransient<IIntegrationEventHandler, TH>(typeof(T));

        services.Configure<EventBusSubscriptionInfo>(o =>
        {
            // Keep track of all registered event types and their name mapping. We send these event types over the message bus
            // and we don't want to do Type.GetType, so we keep track of the name mapping here.

            // This list will also be used to subscribe to events from the underlying message broker implementation.
            o.EventTypes[typeof(T).Name] = typeof(T);
        });

        return services;
    }
}