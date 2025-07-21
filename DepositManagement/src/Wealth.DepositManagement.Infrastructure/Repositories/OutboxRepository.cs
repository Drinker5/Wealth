using Wealth.BuildingBlocks.Application;
using Wealth.DepositManagement.Infrastructure.UnitOfWorks;

namespace Wealth.DepositManagement.Infrastructure.Repositories;

public class OutboxRepository(WealthDbContext dbContext) : IOutboxRepository
{
    public async Task Add(OutboxMessage outboxMessage, CancellationToken cancellationToken)
    {
        await dbContext.OutboxMessages.AddAsync(
            outboxMessage,
            cancellationToken);
    }
}