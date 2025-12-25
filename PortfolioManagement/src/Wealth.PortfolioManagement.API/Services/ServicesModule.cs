using Eventso.Subscription.Hosting;
using Wealth.BuildingBlocks.Infrastructure;
using Wealth.BuildingBlocks.Infrastructure.KafkaConsumer;
using Wealth.PortfolioManagement.Application.Services;
using InstrumentsService = Wealth.InstrumentManagement.InstrumentsService;

namespace Wealth.PortfolioManagement.API.Services;

public class ServicesModule : IServiceModule
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IInstrumentService, GrpcInstrumentService>();
        services.AddGrpcClient<InstrumentsService.InstrumentsServiceClient>(o => { o.Address = new Uri("http://instrument"); })
            .AddServiceDiscovery();

        services.AddSubscriptions((subs, sp) =>
        {
            subs.AddBatch(sp,
                "OperationsTopic",
                new OperationDeserializer());
        });
    }
}