using Eventso.KafkaProducer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Infrastructure;
using Wealth.BuildingBlocks.Infrastructure.KafkaProducer;
using Wealth.InstrumentManagement.Application.Options;

namespace Wealth.InstrumentManagement.Infrastructure;

public sealed class ProducersModule : IServiceModule
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptionsWithValidateOnStart<InstrumentsProducerOptions>()
            .BindConfiguration(InstrumentsProducerOptions.Section);

        services
            .AddSingleton<IKafkaProducer<InstrumentPriceChangedIntegrationEvent>>(sp =>
                new KafkaProducer<InstrumentPriceChangedIntegrationEvent>(
                    sp.GetRequiredService<IProducer>(),
                    sp.GetRequiredService<IOptions<InstrumentsProducerOptions>>().Value.InstrumentPriceChangedTopic,
                    i => (int)i.InstrumentType * (1L << 32) + i.InstrumentId.Value
                ))
            .AddSingleton<IKafkaProducer<BondCouponChangedIntegrationEvent>>(sp =>
                new KafkaProducer<BondCouponChangedIntegrationEvent>(
                    sp.GetRequiredService<IProducer>(),
                    sp.GetRequiredService<IOptions<InstrumentsProducerOptions>>().Value.BondCouponChangedTopic,
                    i => i.BondId.Id
                ))
            .AddSingleton<IKafkaProducer<StockDividendChangedIntegrationEvent>>(sp =>
                new KafkaProducer<StockDividendChangedIntegrationEvent>(
                    sp.GetRequiredService<IProducer>(),
                    sp.GetRequiredService<IOptions<InstrumentsProducerOptions>>().Value.StockDividendChangedTopic,
                    i => i.StockId.Id
                ));
    }
}