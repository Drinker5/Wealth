
using Google.Protobuf;

namespace Wealth.BuildingBlocks.Application;

public interface IIntegrationEventHandler<in TIntegrationEvent> : IIntegrationEventHandler
    where TIntegrationEvent : IMessage
{
    Task Handle(TIntegrationEvent @event, CancellationToken token = default);

    Task IIntegrationEventHandler.Handle(IMessage @event, CancellationToken token) => Handle((TIntegrationEvent)@event, token);
}

public interface IIntegrationEventHandler
{
    Task Handle(IMessage @event, CancellationToken token = default);
}
