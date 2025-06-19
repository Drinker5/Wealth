namespace Wealth.CurrencyManagement.Application.Abstractions;

public interface IEventBus
{
    Task Publish(IntegrationEvent @event, CancellationToken token = default);
}