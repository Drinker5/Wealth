using Wealth.BuildingBlocks.Application;
using Wealth.WalletManagement.Domain.Repositories;

namespace Wealth.WalletManagement.Application.Wallets.Commands;

public class EjectMoneyHandler(IWalletRepository repository) : ICommandHandler<EjectMoney>
{
    public Task Handle(EjectMoney request, CancellationToken cancellationToken)
    {
        return repository.EjectMoney(request.WalletId, request.Money);
    }
}