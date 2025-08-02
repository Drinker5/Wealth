using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Wealth.BuildingBlocks.Infrastructure;
using Wealth.BuildingBlocks.Infrastructure.Mediation.RequestProcessing.CommandBehaviors;
using Wealth.BuildingBlocks.Infrastructure.Mediation.RequestProcessing.QueryPipelines;
using Wealth.StrategyTracking.Infrastructure.Mediation.RequestProcessing.CommandBehaviors;

namespace Wealth.StrategyTracking.Infrastructure.Mediation;

public class MediatorModule : IServiceModule
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
            cfg.AddOpenBehavior(typeof(CommandLoggingBehavior<,>));
            cfg.AddOpenBehavior(typeof(RequestProcessing.CommandBehaviors.CommandUnitOfWorkBehavior<,>));
            cfg.AddOpenBehavior(typeof(CommandDispatchEventsBehavior<,>));

            cfg.AddOpenBehavior(typeof(QueryLoggingPipeline<,>));
        });
    }
}