using System.ComponentModel.DataAnnotations.Schema;
using Wealth.BuildingBlocks.Domain.Utilities;

namespace Wealth.BuildingBlocks.Domain;

public abstract class AggregateRoot : IEntity
{
    private List<DomainEvent>? events;

    [NotMapped]
    public IReadOnlyCollection<DomainEvent>? DomainEvents => events?.AsReadOnly();

    private void AddDomainEvent(DomainEvent domainEvent)
    {
        events ??= [];
        events.Add(domainEvent);
    }

    /// <summary>
    /// Invoke "When" method, then append the event to changes
    /// </summary>
    /// <param name="event"></param>
    protected void Apply(DomainEvent @event)
    {
        this.Invoke("When", @event);
        AddDomainEvent(@event);
    }

    public void ClearDomainEvents()
    {
        events?.Clear();
    }
}