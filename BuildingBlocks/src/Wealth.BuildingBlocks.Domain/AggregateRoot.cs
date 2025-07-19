using Wealth.BuildingBlocks.Domain.Utilities;

namespace Wealth.BuildingBlocks.Domain;

public abstract class AggregateRoot : IEntity
{
    private List<IDomainEvent>? events;
    public IReadOnlyCollection<IDomainEvent>? DomainEvents => events?.AsReadOnly();

    private void AddDomainEvent(IDomainEvent domainEvent)
    {
        events ??= [];
        events.Add(domainEvent);
    }

    /// <summary>
    /// Invoke "When" method, then append the event to changes
    /// </summary>
    /// <param name="event"></param>
    protected void Apply(IDomainEvent @event)
    {
        this.Invoke("When", @event);
        AddDomainEvent(@event);
    }

    public void ClearDomainEvents()
    {
        events?.Clear();
    }
}