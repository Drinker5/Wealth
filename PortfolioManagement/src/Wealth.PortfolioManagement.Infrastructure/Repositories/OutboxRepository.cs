using Wealth.BuildingBlocks.Application;
using Wealth.PortfolioManagement.Infrastructure.UnitOfWorks;

namespace Wealth.PortfolioManagement.Infrastructure.Repositories;

public class OutboxRepository(WealthDbContext dbContext) : IOutboxRepository
{
    public async Task Add(OutboxMessage outboxMessage, CancellationToken cancellationToken)
    {
        await dbContext.OutboxMessages.AddAsync(
            outboxMessage,
            cancellationToken);
    }
}