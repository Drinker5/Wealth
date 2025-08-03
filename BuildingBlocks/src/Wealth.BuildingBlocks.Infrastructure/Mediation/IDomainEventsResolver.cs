using Wealth.BuildingBlocks.Domain;

namespace Wealth.BuildingBlocks.Infrastructure.Mediation;

public interface IDomainEventsResolver
{
    public IReadOnlyCollection<DomainEvent> Resolve();
}