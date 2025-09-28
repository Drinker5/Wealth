using Wealth.BuildingBlocks.Infrastructure;
using Wealth.BuildingBlocks.Infrastructure.KafkaConsumer;
using Wealth.PortfolioManagement.Application.Services;
using Wealth.PortfolioManagement.Infrastructure.Providers.Handling;
using InstrumentsService = Wealth.InstrumentManagement.InstrumentsService;

namespace Wealth.PortfolioManagement.API.Services;

public class ServicesModule : IServiceModule
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IInstrumentService, GrpcInstrumentService>();
        services.AddGrpcClient<InstrumentsService.InstrumentsServiceClient>(o => { o.Address = new Uri("http://instrument"); })
            .AddServiceDiscovery();

        services.AddSingleton<OperationHandler>();
        services.AddHostedService(sp =>
        {
            var kafkaConsumer = sp.GetRequiredService<IKafkaConsumer>();
            var topicOptions = new TopicOptions();
            configuration.GetRequiredSection("OperationsTopic").Bind(topicOptions);
            var handler = sp.GetRequiredService<OperationHandler>();
            return new ConsumerHostedService<Tinkoff.InvestApi.V1.Operation>(
                kafkaConsumer,
                topicOptions,
                handler,
                sp.GetRequiredService<ILogger<ConsumerHostedService<Tinkoff.InvestApi.V1.Operation>>>());
        });
    }
}