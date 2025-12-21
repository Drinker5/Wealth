using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Wealth.BuildingBlocks.Infrastructure;
using Wealth.StrategyTracking.Application.Strategies.ComponentsProvider;

namespace Wealth.StrategyTracking.Infrastructure.Strategies;

public class ComponentsProviderModule : IServiceModule
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<ComponentsProviderFactory>();
        services.AddSingleton<IMoexComponentsProvider, TinkoffMoexComponentProvider>();
        services.Decorate<IMoexComponentsProvider, MoexComponentsProviderDecorator>();
    }
}