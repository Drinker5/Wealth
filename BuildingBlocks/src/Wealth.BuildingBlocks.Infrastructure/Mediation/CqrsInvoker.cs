using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Wealth.BuildingBlocks.Application;

namespace Wealth.BuildingBlocks.Infrastructure.Mediation;

internal class CqrsInvoker(IServiceProvider serviceProvider) : ICqrsInvoker
{
    public async Task<TResult> Command<TResult>(ICommand<TResult> command, CancellationToken token = default)
    {
        using var scope = serviceProvider.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        return await mediator.Send<TResult>(command, token);
    }

    public async Task Command(ICommand command, CancellationToken token = default)
    {
        using var scope = serviceProvider.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        await mediator.Send(command, token);
    }

    public async Task<TResult> Query<TResult>(IQuery<TResult> query, CancellationToken token = default)
    {
        using var scope = serviceProvider.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        return await mediator.Send(query, token);
    }
}
