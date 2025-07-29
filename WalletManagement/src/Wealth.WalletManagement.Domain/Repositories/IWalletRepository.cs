using Wealth.BuildingBlocks.Domain.Common;
using Wealth.WalletManagement.Domain.Wallets;

namespace Wealth.WalletManagement.Domain.Repositories;

public interface IWalletRepository
{
    Task<IReadOnlyCollection<Wallet>> GetWallets();
    Task<Wallet?> GetWallet(WalletId walletId);
    Task<WalletId> Create(string name);
    Task Rename(WalletId walletId, string name);
    Task InsertMoney(WalletId walletId, Money money);
    Task EjectMoney(WalletId walletId, Money money);
}