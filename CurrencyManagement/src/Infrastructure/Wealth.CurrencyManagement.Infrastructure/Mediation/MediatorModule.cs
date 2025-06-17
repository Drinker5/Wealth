using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Wealth.CurrencyManagement.Infrastructure.Interfaces;

namespace Wealth.CurrencyManagement.Infrastructure.Mediation;

public class MediatorModule : IServiceModule
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));
    }
}
