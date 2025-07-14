namespace Wealth.BuildingBlocks.Application;

public interface IOutboxRepository
{
    Task Add(IntegrationEvent integrationEvent, CancellationToken cancellationToken);
}
