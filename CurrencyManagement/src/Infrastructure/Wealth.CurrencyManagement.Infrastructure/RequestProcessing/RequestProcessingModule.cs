using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Wealth.CurrencyManagement.Infrastructure.Interfaces;
using Wealth.CurrencyManagement.Infrastructure.RequestProcessing.CommandPipelines;
using Wealth.CurrencyManagement.Infrastructure.RequestProcessing.QueryPipelines;

namespace Wealth.CurrencyManagement.Infrastructure.RequestProcessing;

public class RequestProcessingModule : IServiceModule
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<CqrsInvoker>();

        // Command Pipelines
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(CommandLoggingPipeline<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(CommandUnitOfWorkPipeline<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(QueryLoggingPipeline<,>));
    }
}