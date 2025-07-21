using Wealth.BuildingBlocks.Application;
using Wealth.DepositManagement.Domain.Deposits.Events;

namespace Wealth.DepositManagement.Application.Deposits.Events.DepositWithdrewHandlers;

public class AddToOutbox(IOutboxRepository repository) : IDomainEventHandler<Domain.Deposits.Events.DepositWithdrew>
{
    public Task Handle(DepositWithdrew notification, CancellationToken cancellationToken)
    {
        return repository.Add(
            notification,
            new DepositWithdrawalIntegrationEvent
            {
                DepositId = notification.DepositId,
                Withdraw = notification.Withdraw,
            },
            notification.DepositId.ToString(),
            cancellationToken);
    }
}