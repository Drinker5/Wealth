using Wealth.BuildingBlocks.Application;
using Wealth.WalletManagement.Domain.Repositories;
using Wealth.WalletManagement.Domain.Wallets;

namespace Wealth.WalletManagement.Application.Wallets.Commands;

public class CreateWalletHandler(IWalletRepository repository) : ICommandHandler<CreateWallet, WalletId>
{
    public Task<WalletId> Handle(CreateWallet request, CancellationToken cancellationToken)
    {
        return repository.Create(request.Name);
    }
}