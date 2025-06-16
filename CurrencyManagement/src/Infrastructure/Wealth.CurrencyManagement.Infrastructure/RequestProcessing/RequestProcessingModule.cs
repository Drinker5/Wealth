using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Wealth.CurrencyManagement.Infrastructure.RequestProcessing.CommandPipelines;

namespace Wealth.CurrencyManagement.Infrastructure.RequestProcessing;

public static class RequestProcessingModule
{
    public static void RegisterRequestProcessingModule(this IServiceCollection services)
    {
        services.AddSingleton<CqrsInvoker>();

        // Command Pipelines
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(CommandLoggingPipeline<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(CommandUnitOfWorkPipeline<,>));
    }
}