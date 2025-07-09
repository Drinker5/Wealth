using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Wealth.BuildingBlocks.Application;

namespace Wealth.BuildingBlocks.Infrastructure.Mediation;

public class CqrsInvoker(IServiceProvider serviceProvider)
{
    public async Task<TResult> Command<TResult>(ICommand<TResult> command)
    {
        using var scope = serviceProvider.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        return await mediator.Send<TResult>(command);
    }

    public async Task Command(ICommand command)
    {
        using var scope = serviceProvider.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        await mediator.Send(command);
    }

    public async Task<TResult> Query<TResult>(IQuery<TResult> query)
    {
        using var scope = serviceProvider.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        return await mediator.Send(query);
    }
}
