namespace Wealth.BuildingBlocks.Application;

public interface IEventBus
{
    Task Publish(IntegrationEvent @event, CancellationToken token = default);
}