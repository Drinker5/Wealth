using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Infrastructure.Mediation.RequestProcessing;
using Wealth.BuildingBlocks.Infrastructure.Mediation.RequestProcessing.CommandBehaviors;
using Wealth.BuildingBlocks.Infrastructure.Mediation.RequestProcessing.QueryPipelines;

namespace Wealth.BuildingBlocks.Infrastructure.Mediation;

internal class MediatorModule : IServiceModule
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
        
        services.AddScoped<IDomainEventsResolver, DummyDomainEventsResolver>();
    }
}