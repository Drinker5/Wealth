using Wealth.BuildingBlocks.Domain;

namespace Wealth.BuildingBlocks.Application;

public interface IOutboxRepository
{
    Task Add(IDomainEvent domainEvent, CancellationToken cancellationToken);
}
