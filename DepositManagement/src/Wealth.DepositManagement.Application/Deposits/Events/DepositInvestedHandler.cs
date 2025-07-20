using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Utilities;
using Wealth.DepositManagement.Domain.DepositOperations;
using Wealth.DepositManagement.Domain.Deposits.Events;
using Wealth.DepositManagement.Domain.Repositories;

namespace Wealth.DepositManagement.Application.Deposits.Events;

public class DepositInvestedHandler(IDepositOperationRepository repository) : IDomainEventHandler<DepositInvested>
{
    public Task Handle(DepositInvested notification, CancellationToken cancellationToken)
    {
        return repository.CreateOperation(
            notification.DepositId,
            DepositOperationType.Investment,
            Clock.Now,
            notification.Investment);
    }
}