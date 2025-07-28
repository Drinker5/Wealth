using Wealth.BuildingBlocks.Application;
using Wealth.WalletManagement.Domain.Repositories;

namespace Wealth.WalletManagement.Application.Wallets.Commands;

public class InsertMoneyHandler(IWalletRepository repository) : ICommandHandler<InsertMoney>
{
    public Task Handle(InsertMoney request, CancellationToken cancellationToken)
    {
        return repository.InsertMoney(request.WalletId, request.Money);
    }
}
