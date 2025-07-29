using Wealth.BuildingBlocks.Domain.Common;
using Wealth.BuildingBlocks.Infrastructure.EFCore.DbSeeding;
using Wealth.WalletManagement.Domain.WalletOperations;
using Wealth.WalletManagement.Domain.Wallets;
using Wealth.WalletManagement.Infrastructure.UnitOfWorks;

namespace Wealth.WalletManagement.Infrastructure.DbSeeding;

public class FirstSeed : IDbSeeder<WealthDbContext>
{
    public async Task SeedAsync(WealthDbContext context)
    {
        if (!context.Wallets.Any())
        {
            await context.Wallets.AddRangeAsync(GetPredefinedWallets());
            await context.WalletOperations.AddRangeAsync(GetPredefinedOperations());
        }

        await context.SaveChangesAsync();
    }

    private static IEnumerable<Wallet> GetPredefinedWallets()
    {
        var foo = Wallet.Create("Test-wallet-1");
        foo.Insert(new Money("RUB", 13.4m));
        foo.Insert(new Money("USD", 43.5m));
        yield return foo;
        
        var bar = Wallet.Create("Test-wallet-2");
        bar.Insert(new Money("RUB", 23.6m));
        bar.Insert(new Money("USD", 33.7m));
        yield return bar;
    }

    private static IEnumerable<WalletOperation> GetPredefinedOperations()
    {
        yield return new WalletOperation
        {
            Id = Guid.NewGuid(),
            Amount = new Money("RUB", 13.4m),
            OperationType = WalletOperationType.Insert,
            WalletId = 1,
        };
        yield return new WalletOperation
        {
            Id = Guid.NewGuid(),
            Amount = new Money("USD", 43.5m),
            OperationType = WalletOperationType.Insert,
            WalletId = 1,
        };
        yield return new WalletOperation
        {
            Id = Guid.NewGuid(),
            Amount = new Money("RUB", 23.6m),
            OperationType = WalletOperationType.Insert,
            WalletId = 2,
        };
        yield return new WalletOperation
        {
            Id = Guid.NewGuid(),
            Amount = new Money("USD", 33.7m),
            OperationType = WalletOperationType.Insert,
            WalletId = 2,
        };
    }
}