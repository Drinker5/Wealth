using Google.Protobuf;
using Wealth.BuildingBlocks.Domain;

namespace Wealth.BuildingBlocks.Application;

public interface IOutboxRepository
{
    Task Add(OutboxMessage integrationEvent, CancellationToken cancellationToken);
}

public static class OutboxRepositoryExtensions
{
    public static Task Add(this IOutboxRepository repository,
        DomainEvent domainEvent,
        IMessage message,
        string? key = null,
        CancellationToken cancellationToken = default)
    {
        return repository.Add(
            domainEvent.ToOutboxMessage(message, key), 
            cancellationToken);
    }
}