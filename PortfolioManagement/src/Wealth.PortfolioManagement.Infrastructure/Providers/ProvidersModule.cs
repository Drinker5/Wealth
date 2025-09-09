using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Wealth.BuildingBlocks.Infrastructure;
using Wealth.PortfolioManagement.Application.Providers;

namespace Wealth.PortfolioManagement.Infrastructure.Providers;

public class ProvidersModule : IServiceModule
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<TBankOperationProviderOptions>()
            .BindConfiguration(TBankOperationProviderOptions.Section)
            .ValidateDataAnnotations()
            .ValidateOnStart();
        
        services.AddSingleton<IOperationProvider, TBankOperationProvider>();
    }
}