using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Wealth.BuildingBlocks.Infrastructure;
using Wealth.InstrumentManagement.Infrastructure.Mediation.CommandBehaviors;

namespace Wealth.InstrumentManagement.Infrastructure.Mediation;

public class MediationModule : IServiceModule
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
            cfg.AddOpenBehavior(typeof(CommandUnitOfWorkBehavior<,>));
            cfg.AddOpenBehavior(typeof(CommandEventDispatchBehavior<,>));
        });
    }
}