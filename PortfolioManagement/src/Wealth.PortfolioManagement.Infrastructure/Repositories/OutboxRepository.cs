using System.Text.Json;
using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain;
using Wealth.BuildingBlocks.Domain.Utilities;
using Wealth.PortfolioManagement.Infrastructure.UnitOfWorks;

namespace Wealth.PortfolioManagement.Infrastructure.Repositories;

public class OutboxRepository : IOutboxRepository
{
    private readonly WealthDbContext dbContext;

    public OutboxRepository(WealthDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task Add(IDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        await dbContext.OutboxMessages.AddAsync(new OutboxMessage
        {
            Id = Guid.NewGuid(),
            OccurredOn = Clock.Now,
            Data = JsonSerializer.Serialize(domainEvent, domainEvent.GetType()),
            Type = domainEvent.GetType().Name,
        }, cancellationToken);
    }
}