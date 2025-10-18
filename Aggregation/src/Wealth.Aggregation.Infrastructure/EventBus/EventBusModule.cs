using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Wealth.Aggregation.Application.Events;
using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Infrastructure;
using Wealth.BuildingBlocks.Infrastructure.KafkaConsumer;
using Wealth.PortfolioManagement;

namespace Wealth.Aggregation.Infrastructure.EventBus;

public class EventBusModule : IServiceModule
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddTopicHandler<StockPriceChangedIntegrationEvent, StockPriceChangedIntegrationEventHandler>(
            configuration, "WealthInstrumentManagement");
        services.AddTopicHandler<OperationProto, OperationEventHandler>(
            configuration, "WealthInstrumentManagement");
    }
}