using Wealth.BuildingBlocks.Application;
using Wealth.DepositManagement.Domain.Deposits.Events;

namespace Wealth.DepositManagement.Application.Deposits.Events.DepositInvestedHandlers;

public class AddToOutbox(IOutboxRepository repository) : IDomainEventHandler<Domain.Deposits.Events.DepositInvested>
{
    public Task Handle(DepositInvested notification, CancellationToken cancellationToken)
    {
        return repository.Add(
            notification,
            new DepositInvestedIntegrationEvent
            {
                DepositId = notification.DepositId,
                Investment = notification.Investment,
            },
            notification.DepositId.ToString(),
            cancellationToken);
    }
}