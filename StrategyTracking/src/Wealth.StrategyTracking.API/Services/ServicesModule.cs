using Wealth.BuildingBlocks.Infrastructure;

namespace Wealth.StrategyTracking.API.Services;

public class ServicesModule : IServiceModule
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddGrpc();
    }
}