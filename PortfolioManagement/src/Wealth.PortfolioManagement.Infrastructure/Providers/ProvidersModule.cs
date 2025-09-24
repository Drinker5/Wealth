using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Wealth.BuildingBlocks.Infrastructure;
using Wealth.PortfolioManagement.Application.Providers;
using Wealth.PortfolioManagement.Infrastructure.Providers.Handling;

namespace Wealth.PortfolioManagement.Infrastructure.Providers;

public class ProvidersModule : IServiceModule
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<TBankOperationProviderOptions>()
            .BindConfiguration(TBankOperationProviderOptions.Section)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddOptions<PortfolioMapOptions>()
            .BindConfiguration(PortfolioMapOptions.Section);

        services.AddSingleton<IOperationProvider, TBankOperationProvider>();
        services.AddSingleton<IOperationProducer, TBankOperationProducer>();
        services.AddSingleton<IPortfolioIdProvider, PortfolioIdProvider>();
        services.AddSingleton<IInstrumentIdProvider, InstrumentIdProvider>();
        services.AddSingleton<OperationConverter>();
    }
}