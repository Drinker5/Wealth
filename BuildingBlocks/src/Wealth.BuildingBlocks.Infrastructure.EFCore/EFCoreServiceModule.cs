using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Infrastructure.Mediation;

namespace Wealth.BuildingBlocks.Infrastructure.EFCore;

internal class EFCoreServiceModule : IServiceModule
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IDomainEventsResolver, EFCoreDomainEventsResolver>();
        services.AddScoped<IUnitOfWork, EFCoreUnitOfWork>();
    }
}