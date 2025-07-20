using Wealth.BuildingBlocks.Domain.Common;
using Wealth.DepositManagement.Domain.DepositOperations;
using Wealth.DepositManagement.Domain.Deposits;
using Wealth.DepositManagement.Domain.Repositories;
using Wealth.DepositManagement.Infrastructure.UnitOfWorks;

namespace Wealth.DepositManagement.Infrastructure.Repositories;

public class DepositOperationRepository(WealthDbContext context) : IDepositOperationRepository
{
    public async Task<Guid> CreateOperation(
        DepositId depositId,
        DepositOperationType type,
        DateTimeOffset date,
        Money money)
    {
        var depositOperation = new DepositOperation
        {
            DepositId = depositId,
            Type = type,
            Date = date,
            Money = money,
        };
        
        await context.DepositOperations.AddAsync(depositOperation);
        return depositOperation.Id;
    }
}