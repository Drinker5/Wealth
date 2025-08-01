using Microsoft.EntityFrameworkCore;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.WalletManagement.Domain.Repositories;
using Wealth.WalletManagement.Domain.Wallets;
using Wealth.WalletManagement.Infrastructure.UnitOfWorks;

namespace Wealth.WalletManagement.Infrastructure.Repositories;

public class WalletRepository(WealthDbContext context) : IWalletRepository
{
    public async Task<IReadOnlyCollection<Wallet>> GetWallets()
    {
        return await context.Wallets.AsNoTracking()
            .Include(i => i.Currencies)
            .ToListAsync();
    }

    public Task<Wallet?> GetWallet(WalletId id)
    {
        return context.Wallets
            .Include(i => i.Currencies)
            .SingleOrDefaultAsync(i => i.Id == id);
    }

    public async Task<WalletId> Create(string name)
    {
        var wallet = Wallet.Create(name);
        await context.Wallets.AddAsync(wallet);
        return wallet.Id;
    }

    public async Task Rename(WalletId walletId, string name)
    {
        var wallet = await GetWallet(walletId);

        wallet?.Rename(name);
    }

    public async Task InsertMoney(WalletId walletId, Money money)
    {
        var wallet = await GetWallet(walletId);

        wallet?.Insert(money);
    }

    public async Task EjectMoney(WalletId walletId, Money money)
    {
        var wallet = await GetWallet(walletId);

        wallet?.Eject(money);
    }
}