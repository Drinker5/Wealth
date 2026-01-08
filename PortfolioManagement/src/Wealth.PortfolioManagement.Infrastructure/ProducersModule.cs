using Eventso.KafkaProducer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Infrastructure;
using Wealth.BuildingBlocks.Infrastructure.KafkaProducer;
using Wealth.PortfolioManagement.Application.Options;

namespace Wealth.PortfolioManagement.Infrastructure;

public sealed class ProducersModule : IServiceModule
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptionsWithValidateOnStart<OperationsProducerOptions>()
            .BindConfiguration(OperationsProducerOptions.Section);

        services
            .AddSingleton<IKafkaProducer<Tinkoff.InvestApi.V1.Operation>>(sp =>
                new KafkaProducer<Tinkoff.InvestApi.V1.Operation>(
                    sp.GetRequiredService<IProducer>(),
                    "wealth-operations",
                    i => i.Date.Seconds
                ))
            .AddSingleton<IKafkaProducer<OperationProto>>(sp =>
                new KafkaProducer<OperationProto>(
                    sp.GetRequiredService<IProducer>(),
                    sp.GetRequiredService<IOptions<OperationsProducerOptions>>().Value.Topic,
                    i => i.Date.Seconds
                ));
    }
}