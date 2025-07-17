using Wealth.BuildingBlocks.Application;
using Wealth.PortfolioManagement.Infrastructure.UnitOfWorks;

namespace Wealth.PortfolioManagement.Infrastructure.Repositories;

public class OutboxRepository(WealthDbContext dbContext) : IOutboxRepository
{
    public async Task Add(IntegrationEvent integrationEvent, CancellationToken cancellationToken)
    {
        await dbContext.OutboxMessages.AddAsync(
            integrationEvent.ToOutboxMessage(),
            cancellationToken);
    }
}