using Wealth.BuildingBlocks.Application;
using Wealth.WalletManagement.Domain.Repositories;
using Wealth.WalletManagement.Domain.Wallets;

namespace Wealth.WalletManagement.Application.Wallets.Queries;

public class GetWalletsHandler(IWalletRepository repository) : IQueryHandler<GetWallets, IReadOnlyCollection<Wallet>>
{
    public Task<IReadOnlyCollection<Wallet>> Handle(GetWallets request, CancellationToken cancellationToken)
    {
        return repository.GetWallets();
    }
}