using Wealth.BuildingBlocks.Application;
using Wealth.WalletManagement.Domain.Wallets;

namespace Wealth.WalletManagement.Application.Wallets.Events;

public class WalletMoneyEjectedHandler1(IOutboxRepository repository) : IDomainEventHandler<WalletMoneyEjected>
{
    public async Task Handle(WalletMoneyEjected notification, CancellationToken cancellationToken)
    {
        await repository.Add(
            notification.ToOutboxMessage(
                new WalletMoneyEjectedDomainEvent
                {
                    WalletId = notification.WalletId,
                    Money = notification.Money
                },
                notification.WalletId.ToString()),
            cancellationToken);
    }
}