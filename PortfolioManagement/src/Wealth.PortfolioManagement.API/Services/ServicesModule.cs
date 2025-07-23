using Wealth.BuildingBlocks.Application.Services;
using Wealth.BuildingBlocks.Infrastructure;
using Wealth.InstrumentManagement;

namespace Wealth.PortfolioManagement.API.Services;

public class ServicesModule : IServiceModule
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IInstrumentService, GrpcInstrumentService>();
        services.AddGrpcClient<InstrumentsService.InstrumentsServiceClient>(o =>
        {
            o.Address = new Uri("http://instrument");
        }).AddServiceDiscovery();
    }
}