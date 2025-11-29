using Wealth.BuildingBlocks.Infrastructure;
using Wealth.InstrumentManagement;

namespace Wealth.Aggregation.API.Services;

public class GrpcServicesModule : IServiceModule
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddGrpcClient<InstrumentsService.InstrumentsServiceClient>(o =>
        {
            o.Address = new Uri("http://instrument");
        }).AddServiceDiscovery();
    }
}