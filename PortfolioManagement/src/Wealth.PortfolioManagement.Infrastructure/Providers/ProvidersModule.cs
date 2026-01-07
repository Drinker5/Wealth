using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Wealth.BuildingBlocks.Infrastructure;
using Wealth.PortfolioManagement.Application.Options;
using Wealth.PortfolioManagement.Application.Providers;
using Wealth.PortfolioManagement.Infrastructure.Providers.Handling;
using Wealth.PortfolioManagement.Infrastructure.Providers.Handling.Handlers;

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

        services.AddOptions<OperationsProducerOptions>()
            .BindConfiguration(OperationsProducerOptions.Section)
            .ValidateDataAnnotations()
            .ValidateOnStart();
        
        services.AddSingleton<IOperationProducer, TBankOperationProducer>();
        services.AddSingleton<TBankOperationProvider>();
        services.AddSingleton<IPortfolioIdProvider, PortfolioIdProvider>();
        services.AddSingleton<IInstrumentIdProvider, InstrumentIdProvider>();
        services.Decorate<IInstrumentIdProvider, InstrumentIdProviderCacheDecorator>();
        services.AddSingleton<OperationConverter>();
        RegisterOperationHandlers(services);
    }

    private static void RegisterOperationHandlers(IServiceCollection services)
    {
        var handlerTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false } && 
                        t.GetInterfaces().Contains(typeof(IOperationHandler)) &&
                        t.Namespace == typeof(SellHandler).Namespace)
            .ToList();

        foreach (var handlerType in handlerTypes)
            services.AddSingleton(handlerType);
    }
}