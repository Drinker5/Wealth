using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Wealth.BuildingBlocks.Infrastructure;
using Wealth.InstrumentManagement.Application.Providers;

namespace Wealth.InstrumentManagement.Infrastructure.Providers;

public class ProvidersModule : IServiceModule
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<TBankInstrumentsProviderOptions>()
            .BindConfiguration(TBankInstrumentsProviderOptions.Section)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddSingleton<IInstrumentsProvider, InstrumentsProvider>();
        services.Decorate<IInstrumentsProvider, InstrumentsProviderDecorator>();
    }
}