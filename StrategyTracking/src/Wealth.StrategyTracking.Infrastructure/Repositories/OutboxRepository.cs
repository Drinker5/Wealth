using Wealth.BuildingBlocks.Application;
using Wealth.StrategyTracking.Infrastructure.UnitOfWorks;

namespace Wealth.StrategyTracking.Infrastructure.Repositories;

public class OutboxRepository(WealthDbContext dbContext) : IOutboxRepository
{
    public async Task Add(OutboxMessage outboxMessage, CancellationToken cancellationToken)
    {
        await dbContext.OutboxMessages.AddAsync(
            outboxMessage,
            cancellationToken);
    }
}