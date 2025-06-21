using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Wealth.CurrencyManagement.Application.Abstractions;
using Wealth.CurrencyManagement.Application.DataProviders;

namespace Wealth.CurrencyManagement.Infrastructure.DataProviders;

public class DataProvidersModule : IServiceModule
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        services.AddSingleton<IExchangeRateDataProvider, CbrExchangeRateDataProvider>();
        services.AddSingleton<HttpClient>();
        services.AddMemoryCache();
    }
}