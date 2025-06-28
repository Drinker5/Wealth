namespace Wealth.BuildingBlocks.Application;

public abstract record IntegrationEvent
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public DateTime CreationDate { get; set; } = DateTime.UtcNow;
}