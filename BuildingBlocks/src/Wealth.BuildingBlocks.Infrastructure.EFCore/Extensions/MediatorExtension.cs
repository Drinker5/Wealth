using MediatR;
using Microsoft.EntityFrameworkCore;
using Wealth.BuildingBlocks.Domain;

namespace Wealth.BuildingBlocks.Infrastructure.EFCore.Extensions;

public static class MediatorExtension
{
    public static async Task DispatchDomainEvents(this IMediator mediator, DbContext ctx)
    {
        var domainEntities = ctx.ChangeTracker
            .Entries<AggregateRoot>()
            .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Count != 0);

        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.DomainEvents!)
            .ToList();

        foreach (var entity in domainEntities)
            entity.Entity.ClearDomainEvents();

        foreach (var domainEvent in domainEvents)
            await mediator.Publish(domainEvent);
    }
}