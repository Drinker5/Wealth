using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Wealth.Aggregation.Application.Events;
using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Infrastructure;

namespace Wealth.Aggregation.Infrastructure.EventBus;

public class EventBusModule : IServiceModule
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<KafkaConsumerOptions>(configuration.GetSection(KafkaConsumerOptions.Section));

        services.AddHostedService<KafkaConsumer>();
        services.AddSubscription<StockBoughtIntegrationEvent, StockBoughtIntegrationEventHandler>();
        services.AddSubscription<InstrumentPriceChangedIntegrationEvent, InstrumentPriceChangedIntegrationEventHandler>();
    }
}