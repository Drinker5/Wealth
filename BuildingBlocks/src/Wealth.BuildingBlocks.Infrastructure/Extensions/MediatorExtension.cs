using MediatR;
using Wealth.BuildingBlocks.Infrastructure.Mediation;

namespace Wealth.BuildingBlocks.Infrastructure.Extensions;

public static class MediatorExtension
{
    public static async Task DispatchDomainEvents(this IMediator mediator, IDomainEventsResolver domainEvents)
    {
        foreach (var domainEvent in domainEvents.Resolve())
            await mediator.Publish(domainEvent);
    }
}