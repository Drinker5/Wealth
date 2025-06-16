using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Wealth.CurrencyManagement.Application.Interfaces;

namespace Wealth.CurrencyManagement.Infrastructure;

public class CqrsInvoker
{
    private readonly IServiceProvider _serviceProvider;

    public CqrsInvoker(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public async Task<TResult> CommandAsync<TResult>(ICommand<TResult> command)
    {
        using var scope = _serviceProvider.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        return await mediator.Send(command);
    }

    public async Task CommandAsync(ICommand command)
    {
        using var scope = _serviceProvider.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        await mediator.Send(command);
    }

    public async Task<TResult> QueryAsync<TResult>(IQuery<TResult> query)
    {
        using var scope = _serviceProvider.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        return await mediator.Send(query);
    }
}
