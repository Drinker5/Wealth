using Wealth.BuildingBlocks.Infrastructure;
using Wealth.InstrumentManagement;

namespace Wealth.StrategyTracking.API.Services;

public class ServicesModule : IServiceModule
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddGrpcClient<InstrumentsService.InstrumentsServiceClient>(o => { o.Address = new Uri("http://instrument"); })
            .AddServiceDiscovery();

        services.AddGrpc();
    }
}