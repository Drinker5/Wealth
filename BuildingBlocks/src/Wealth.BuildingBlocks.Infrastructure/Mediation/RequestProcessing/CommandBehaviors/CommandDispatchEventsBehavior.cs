using MediatR;
using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Infrastructure.Extensions;

namespace Wealth.BuildingBlocks.Infrastructure.Mediation.RequestProcessing.CommandBehaviors;

internal class CommandDispatchEventsBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand
{
    private readonly IMediator mediator;
    private readonly IDomainEventsResolver domainEventsResolver;

    public CommandDispatchEventsBehavior(IMediator mediator, IDomainEventsResolver domainEventsResolver)
    {
        this.mediator = mediator;
        this.domainEventsResolver = domainEventsResolver;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var result = await next(cancellationToken);
        await mediator.DispatchDomainEvents(domainEventsResolver);
        return result;
    }
}