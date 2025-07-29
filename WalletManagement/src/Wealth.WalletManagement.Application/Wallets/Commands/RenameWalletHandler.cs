using Wealth.BuildingBlocks.Application;
using Wealth.WalletManagement.Domain.Repositories;

namespace Wealth.WalletManagement.Application.Wallets.Commands;

public class RenameWalletHandler(IWalletRepository repository) : ICommandHandler<RenameWallet>
{
    public Task Handle(RenameWallet request, CancellationToken cancellationToken)
    {
        return repository.Rename(request.WalletId, request.NewName);
    }
}