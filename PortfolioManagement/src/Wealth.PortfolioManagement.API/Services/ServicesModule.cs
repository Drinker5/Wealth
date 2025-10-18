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

        services.AddTopicHandler<Tinkoff.InvestApi.V1.Operation, OperationHandler>(configuration, "OperationsTopic");
    }
}   