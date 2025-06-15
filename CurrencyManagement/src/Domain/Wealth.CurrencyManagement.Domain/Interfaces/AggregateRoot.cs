namespace Wealth.CurrencyManagement.Domain.Interfaces;

public abstract class AggregateRoot
{
    public readonly List<IDomainEvent> Events = [];
}