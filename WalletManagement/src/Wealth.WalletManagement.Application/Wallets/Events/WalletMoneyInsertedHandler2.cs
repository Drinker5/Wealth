using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Utilities;
using Wealth.WalletManagement.Domain.Repositories;
using Wealth.WalletManagement.Domain.WalletOperations;
using Wealth.WalletManagement.Domain.Wallets;

namespace Wealth.WalletManagement.Application.Wallets.Events;

public class WalletMoneyInsertedHandler2(IWalletOperationRepository repository) : IDomainEventHandler<WalletMoneyInserted>
{
    public async Task Handle(WalletMoneyInserted notification, CancellationToken cancellationToken)
    {
        await repository.AddOperation(new WalletOperation
        {
            Id = Guid.NewGuid(),
            WalletId = notification.WalletId,
            Amount = notification.Money,
            Date = Clock.Now,
            OperationType = WalletOperationType.Insert,
        });
    }
}