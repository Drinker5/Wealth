namespace Wealth.CurrencyManagement.Domain.Abstractions;

public interface IEventBus
{
    Task Publish(IntegrationEvent @event);
}