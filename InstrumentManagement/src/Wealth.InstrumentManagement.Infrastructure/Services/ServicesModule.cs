using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Wealth.BuildingBlocks.Infrastructure;
using Wealth.InstrumentManagement.Application.Services;

namespace Wealth.InstrumentManagement.Infrastructure.Services;

public class ServicesModule : IServiceModule
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient<ICurrencyService, CurrencyService>(client =>
        {
            client.BaseAddress = new("http://currency");
        });
        services.AddServiceDiscovery();
    }
}