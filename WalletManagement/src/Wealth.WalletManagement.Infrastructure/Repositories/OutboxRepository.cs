using Wealth.BuildingBlocks.Application;
using Wealth.WalletManagement.Infrastructure.UnitOfWorks;

namespace Wealth.WalletManagement.Infrastructure.Repositories;

public class OutboxRepository(WealthDbContext dbContext) : IOutboxRepository
{
    public async Task Add(OutboxMessage outboxMessage, CancellationToken cancellationToken)
    {
        await dbContext.OutboxMessages.AddAsync(
            outboxMessage,
            cancellationToken);
    }
}