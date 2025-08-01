using Wealth.BuildingBlocks.Infrastructure;
using Wealth.InstrumentManagement;
using Wealth.PortfolioManagement.Application.Services;

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