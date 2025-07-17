using Google.Protobuf;

namespace Wealth.BuildingBlocks.Application;

public interface IOutboxRepository
{
    Task Add(IntegrationEvent integrationEvent, CancellationToken cancellationToken);
}

public static class OutboxRepositoryExtensions
{
    public static Task Add(this IOutboxRepository repository,
        IMessage integrationEvent,
        string? key = null,
        CancellationToken cancellationToken = default)
    {
        return repository.Add(IntegrationEvent.Create(integrationEvent, key), cancellationToken);
    }
}