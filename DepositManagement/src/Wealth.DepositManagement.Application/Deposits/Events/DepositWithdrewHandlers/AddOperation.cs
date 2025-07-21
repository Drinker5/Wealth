using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Utilities;
using Wealth.DepositManagement.Domain.DepositOperations;
using Wealth.DepositManagement.Domain.Deposits.Events;
using Wealth.DepositManagement.Domain.Repositories;

namespace Wealth.DepositManagement.Application.Deposits.Events.DepositWithdrewHandlers;

public class AddOperation(IDepositOperationRepository repository) : IDomainEventHandler<Domain.Deposits.Events.DepositWithdrew>
{
    public Task Handle(DepositWithdrew notification, CancellationToken cancellationToken)
    {
        return repository.CreateOperation(
            notification.DepositId,
            DepositOperationType.Withdrawal,
            Clock.Now,
            notification.Withdraw);
    }
}