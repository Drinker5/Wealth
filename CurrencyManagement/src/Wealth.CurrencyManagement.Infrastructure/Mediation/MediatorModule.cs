using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Infrastructure;
using Wealth.CurrencyManagement.Application.Abstractions;
using Wealth.CurrencyManagement.Application.Validators;
using Wealth.CurrencyManagement.Infrastructure.Mediation.RequestProcessing;
using Wealth.CurrencyManagement.Infrastructure.Mediation.RequestProcessing.CommandBehaviors;
using Wealth.CurrencyManagement.Infrastructure.Mediation.RequestProcessing.QueryPipelines;

namespace Wealth.CurrencyManagement.Infrastructure.Mediation;

public class MediatorModule : IServiceModule
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ICommandsScheduler, CommandsScheduler>();
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
            cfg.AddOpenBehavior(typeof(CommandLoggingBehavior<,>));
            cfg.AddOpenBehavior(typeof(CommandValidationBehavior<,>));
            cfg.AddOpenBehavior(typeof(CommandUnitOfWorkBehavior<,>));

            cfg.AddOpenBehavior(typeof(QueryLoggingPipeline<,>));
        });
        
        services.AddValidatorsFromAssemblyContaining<CommandValidator<ICommand>>();
    }
}