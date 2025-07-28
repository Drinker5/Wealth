using Wealth.BuildingBlocks.Application;
using Wealth.WalletManagement.Domain.Wallets;

namespace Wealth.WalletManagement.Application.Wallets.Events;

public class WalletMoneyInsertedHandler1(IOutboxRepository repository) : IDomainEventHandler<WalletMoneyInserted>
{
    public async Task Handle(WalletMoneyInserted notification, CancellationToken cancellationToken)
    {
        await repository.Add(
            notification.ToOutboxMessage(
                new WalletMoneyInsertedDomainEvent
                {
                    WalletId = notification.WalletId,
                    Money = notification.Money
                },
                notification.WalletId.ToString()),
            cancellationToken);
    }
}