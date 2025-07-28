using Wealth.BuildingBlocks.Domain.Common;
using Wealth.WalletManagement.Domain.Wallets;

namespace Wealth.WalletManagement.Domain.Repositories;

public interface IWalletRepository
{
    Task<WalletId> Create(string name);
    Task Rename(string name);
    Task InsertMoney(WalletId walletId, Money money);
    Task EjectMoney(WalletId walletId, Money money);
    Task<IReadOnlyCollection<Wallet>> GetWallets();
}