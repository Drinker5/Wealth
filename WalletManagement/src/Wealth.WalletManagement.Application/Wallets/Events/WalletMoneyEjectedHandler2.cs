using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Utilities;
using Wealth.WalletManagement.Domain.Repositories;
using Wealth.WalletManagement.Domain.WalletOperations;
using Wealth.WalletManagement.Domain.Wallets;

namespace Wealth.WalletManagement.Application.Wallets.Events;

public class WalletMoneyEjectedHandler2(IWalletOperationRepository repository) : IDomainEventHandler<WalletMoneyEjected>
{
    public async Task Handle(WalletMoneyEjected notification, CancellationToken cancellationToken)
    {
        await repository.AddOperation(new WalletOperation
        {
            Id = Guid.NewGuid(),
            WalletId = notification.WalletId,
            Amount = notification.Money,
            Date = Clock.Now,
            OperationType = WalletOperationType.Eject,
        });
    }
}