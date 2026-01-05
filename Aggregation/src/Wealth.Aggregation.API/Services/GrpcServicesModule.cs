using Wealth.BuildingBlocks.Infrastructure;
using Wealth.InstrumentManagement;
using Wealth.StrategyTracking;

namespace Wealth.Aggregation.API.Services;

public class GrpcServicesModule : IServiceModule
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddGrpcClient<InstrumentsService.InstrumentsServiceClient>(o =>
        {
            o.Address = new Uri("http://instrument");
        }).AddServiceDiscovery();

        services.AddGrpcClient<StrategiesService.StrategiesServiceClient>(o =>
        {
            o.Address = new Uri("http://strategy");
        }).AddServiceDiscovery();
        
        services.AddServiceDiscovery();
        
    }
}