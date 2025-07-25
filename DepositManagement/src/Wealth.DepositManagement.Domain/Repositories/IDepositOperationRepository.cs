using Wealth.BuildingBlocks.Domain.Common;
using Wealth.DepositManagement.Domain.DepositOperations;

namespace Wealth.DepositManagement.Domain.Repositories;

public interface IDepositOperationRepository
{
    Task<Guid> CreateOperation(
        DepositId depositId,
        DepositOperationType type,
        DateTimeOffset date,
        Money money);
}