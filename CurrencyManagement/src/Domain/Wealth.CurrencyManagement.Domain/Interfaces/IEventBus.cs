namespace Wealth.CurrencyManagement.Domain.Interfaces;

public interface IEventBus
{
    Task PublishAsync(IDomainEvent @event);
}