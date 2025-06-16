using Wealth.CurrencyManagement.Domain.Utilities;

namespace Wealth.CurrencyManagement.Domain.Interfaces;

public abstract class AggregateRoot
{
    private readonly List<IDomainEvent> events = [];
    public IReadOnlyCollection<IDomainEvent> DomainEvents => events.AsReadOnly();

    private void AddDomainEvent(IDomainEvent domainEvent)
    {
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
}