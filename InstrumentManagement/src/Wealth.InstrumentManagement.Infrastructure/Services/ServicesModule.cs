using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Wealth.BuildingBlocks.Infrastructure;

namespace Wealth.InstrumentManagement.Infrastructure.Services;

public class ServicesModule : IServiceModule
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddServiceDiscovery();
    }
}