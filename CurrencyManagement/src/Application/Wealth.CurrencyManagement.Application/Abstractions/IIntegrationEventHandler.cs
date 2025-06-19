namespace Wealth.CurrencyManagement.Application.Abstractions;

public interface IIntegrationEventHandler<in TIntegrationEvent> : IIntegrationEventHandler
    where TIntegrationEvent : IntegrationEvent
{
    Task Handle(TIntegrationEvent @event, CancellationToken token = default);

    Task IIntegrationEventHandler.Handle(IntegrationEvent @event, CancellationToken token) => Handle((TIntegrationEvent)@event, token);
}

public interface IIntegrationEventHandler
{
    Task Handle(IntegrationEvent @event, CancellationToken token = default);
}
