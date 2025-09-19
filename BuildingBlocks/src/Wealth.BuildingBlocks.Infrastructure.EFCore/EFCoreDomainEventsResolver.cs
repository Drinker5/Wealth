using Microsoft.EntityFrameworkCore;
using Wealth.BuildingBlocks.Domain;
using Wealth.BuildingBlocks.Infrastructure.Mediation;

namespace Wealth.BuildingBlocks.Infrastructure.EFCore;

internal class EFCoreDomainEventsResolver(DbContext ctx) : IDomainEventsResolver
{
    public IReadOnlyCollection<DomainEvent> Resolve()
    {
        var domainEntities = ctx.ChangeTracker
            .Entries<AggregateRoot>()
            .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Count != 0);

        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.DomainEvents!)
            .ToList();

        foreach (var entity in domainEntities)
            entity.Entity.ClearDomainEvents();
        
        return domainEvents;
    }
}