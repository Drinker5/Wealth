using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Wealth.CurrencyManagement.Application.Abstractions;

namespace Wealth.CurrencyManagement.Infrastructure.Mediation;

public class CqrsInvoker
{
    private readonly IServiceProvider _serviceProvider;

    public CqrsInvoker(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public async Task<TResult> Command<TResult>(ICommand<TResult> command)
    {
        using var scope = _serviceProvider.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        return await mediator.Send<TResult>(command);
    }

    public async Task Command(ICommand command)
    {
        using var scope = _serviceProvider.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        await mediator.Send(command);
    }

    public async Task<TResult> Query<TResult>(IQuery<TResult> query)
    {
        using var scope = _serviceProvider.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        return await mediator.Send(query);
    }
}
