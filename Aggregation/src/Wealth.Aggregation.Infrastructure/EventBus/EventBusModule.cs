using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Wealth.BuildingBlocks.Infrastructure;
using Wealth.BuildingBlocks.Infrastructure.KafkaConsumer;
using Eventso.Subscription.Hosting;

namespace Wealth.Aggregation.Infrastructure.EventBus;

public class EventBusModule : IServiceModule
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSubscriptions((subs, sp) =>
            {
                subs.AddBatch(sp,
                    "InstrumentsConsumer",
                    new InstrumentManagementEventDeserializer());
                subs.AddBatch(sp,
                    "WealthOperationsConverted",
                    new OperationProtoDeserializer());
            },
            types => types.FromApplicationDependencies(),
            handlersLifetime: ServiceLifetime.Singleton);
    }
}