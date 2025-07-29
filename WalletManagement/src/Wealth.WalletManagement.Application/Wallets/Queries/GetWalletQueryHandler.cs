using Wealth.BuildingBlocks.Application;
using Wealth.WalletManagement.Domain.Repositories;
using Wealth.WalletManagement.Domain.Wallets;

namespace Wealth.WalletManagement.Application.Wallets.Queries;

public class GetWalletQueryHandler(IWalletRepository repository) : IQueryHandler<GetWallet, WalletDTO?>
{
    public async Task<WalletDTO?> Handle(GetWallet request, CancellationToken cancellationToken)
    {
        var wallet = await repository.GetWallet(request.WalletId);
        return wallet != null ? WalletDTO.ToDTO(wallet) : null;
    }
}