using MediatR;
using Wealth.BuildingBlocks.Application;
using Wealth.InstrumentManagement.Infrastructure.UnitOfWorks;

namespace Wealth.InstrumentManagement.Infrastructure.Mediation.CommandBehaviors;

public class CommandEventDispatchBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand
{
    private readonly WealthDbContext dbContext;
    private readonly IMediator mediator;

    public CommandEventDispatchBehavior(WealthDbContext dbContext, IMediator mediator)
    {
        this.dbContext = dbContext;
        this.mediator = mediator;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var result = await next(cancellationToken);
        foreach (var @event in dbContext.TrackedEvents)
            await mediator.Publish(@event, cancellationToken);

        return result;
    }
}