using Wealth.BuildingBlocks.Application;
using Wealth.WalletManagement.Domain.Repositories;

namespace Wealth.WalletManagement.Application.Wallets.Queries;

public class GetWalletsHandler(IWalletRepository repository) : IQueryHandler<GetWallets, IEnumerable<WalletDTO>>
{
    public async Task<IEnumerable<WalletDTO>> Handle(GetWallets request, CancellationToken cancellationToken)
    {
        var wallets = await repository.GetWallets();
        return wallets.Select(WalletDTO.ToDTO);
    }
}