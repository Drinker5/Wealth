using Google.Protobuf;
using Microsoft.Extensions.DependencyInjection;
using Wealth.BuildingBlocks.Application;

namespace Wealth.BuildingBlocks.Infrastructure.EventBus;

public static class EventBusModuleExtensions
{
    public static IServiceCollection AddSubscription<T, TH>(this IServiceCollection services)
        where T : IMessage
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