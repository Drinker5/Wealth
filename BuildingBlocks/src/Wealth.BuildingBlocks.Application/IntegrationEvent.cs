using Wealth.BuildingBlocks.Domain.Utilities;

namespace Wealth.BuildingBlocks.Application;

public abstract record IntegrationEvent
{
    public Guid Id { get; } = Guid.NewGuid();

    public DateTimeOffset CreationDate { get; } = Clock.Now;
}