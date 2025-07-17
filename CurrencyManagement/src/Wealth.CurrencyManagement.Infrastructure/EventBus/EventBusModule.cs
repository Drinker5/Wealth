using System.Net.Sockets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Infrastructure;

namespace Wealth.CurrencyManagement.Infrastructure.EventBus;

public class EventBusModule : IServiceModule
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IEventBus, RabbitMQEventBus>();
        // Start consuming messages as soon as the application starts
        services.AddSingleton<IHostedService>(sp => (RabbitMQEventBus)sp.GetRequiredService<IEventBus>());

        services.Configure<EventBusOptions>(configuration.GetSection(EventBusOptions.Section));

        AddRabbitMQClient(services, configuration);
    }

    private void AddRabbitMQClient(IServiceCollection services, IConfiguration configuration)
    {
        var configSection = configuration.GetSection(EventBusOptions.Section);
        var settings = new EventBusOptions();
        configSection.Bind(settings);

        services.AddSingleton<IConnectionFactory>(sp => CreateConnectionFactory(configSection));
        services.AddSingleton<IConnection>(sp =>
            CreateConnection(sp, settings.RetryCount).AsTask().GetAwaiter().GetResult());
    }

    private IConnectionFactory CreateConnectionFactory(IConfigurationSection configSection)
    {
        var factory = new ConnectionFactory();
        var configurationOptionsSection = configSection.GetSection("ConnectionFactory");
        configurationOptionsSection.Bind(factory);
        return factory;
    }

    private static ValueTask<IConnection> CreateConnection(IServiceProvider sp, int retryCount, CancellationToken cancellationToken = default)
    {
        var resiliencePipelineBuilder = new ResiliencePipelineBuilder();
        var logger = sp.GetRequiredService<ILogger<EventBusModule>>();
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
                OnRetry = args =>
                {
                    logger.LogWarning("Retrying RabbitMQ connection, attempt: {Attempt}, next in: {Delay}", args.AttemptNumber, args.RetryDelay);
                    return ValueTask.CompletedTask;
                },
            });
        }

        var resiliencePipeline = resiliencePipelineBuilder.Build();

        var factory = sp.GetRequiredService<IConnectionFactory>();
        return resiliencePipeline.ExecuteAsync(static async (factory, cancellationToken) =>
            await factory.CreateConnectionAsync(cancellationToken), factory, cancellationToken);
    }
}