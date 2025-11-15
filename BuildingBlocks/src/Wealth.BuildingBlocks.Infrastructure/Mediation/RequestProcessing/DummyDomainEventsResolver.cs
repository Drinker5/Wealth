using Wealth.BuildingBlocks.Domain;

namespace Wealth.BuildingBlocks.Infrastructure.Mediation.RequestProcessing;

internal class DummyDomainEventsResolver: IDomainEventsResolver
{
    public IReadOnlyCollection<DomainEvent> Resolve()
    {
        return [];
    }
}