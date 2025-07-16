using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Wealth.Aggregation.Infrastructure.Mediation.RequestProcessing.CommandBehaviors;
using Wealth.Aggregation.Infrastructure.Mediation.RequestProcessing.QueryPipelines;
using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Infrastructure;
using Wealth.BuildingBlocks.Infrastructure.Mediation;

namespace Wealth.Aggregation.Infrastructure.Mediation;

public class MediatorModule : IServiceModule
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<ICqrsInvoker, CqrsInvoker>();
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
            cfg.AddOpenBehavior(typeof(CommandLoggingBehavior<,>));
            cfg.AddOpenBehavior(typeof(CommandUnitOfWorkBehavior<,>));
            cfg.AddOpenBehavior(typeof(CommandDispatchEventsBehavior<,>));

            cfg.AddOpenBehavior(typeof(QueryLoggingPipeline<,>));
        });
    }
}