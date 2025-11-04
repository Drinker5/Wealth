using Microsoft.EntityFrameworkCore;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.WalletManagement.Domain.Wallets;
using Wealth.WalletManagement.Infrastructure.Repositories;
using Wealth.WalletManagement.Infrastructure.UnitOfWorks;

namespace Wealth.WalletManagement.Infrastructure.Tests.Repositories;

[TestSubject(typeof(WalletRepository))]
public class WalletRepositoryTests
{
    private readonly WalletRepository repository;
    private readonly WealthDbContext context;

    public WalletRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<WealthDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        context = new WealthDbContext(options);
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
        
        repository = new WalletRepository(context);
    }

    [Fact]
    public async Task WhenGetWallets()
    {
        var portfolios = await repository.GetWallets();
        
        Assert.Empty(portfolios);
    }

    [Fact]
    public async Task WhenWalletCreatedThenIdGenerated()
    {
        var portfolio = Wallet.Create("Foo");

        await context.Wallets.AddAsync(portfolio);

        Assert.NotEqual(0, portfolio.Id.Id);
    }

    [Fact]
    public async Task WhenWalletCreatedThenIdReturned()
    {
        var id = await CreateWallet("Foo");
        
        Assert.NotEqual(0, id.Id);
        var portfolio = await repository.GetWallet(id);
        Assert.NotNull(portfolio);
        Assert.Equal("Foo", portfolio.Name);
    }

    [Fact]
    public async Task WhenInsertMoney()
    {
        var money = new Money(CurrencyCode.RUB, 23m);
        var id = await CreateWallet("Foo");

        await repository.InsertMoney(id, money);

        var portfolio = await repository.GetWallet(id);
        Assert.NotNull(portfolio);
        var currency = Assert.Single(portfolio.Currencies);
        Assert.Equal(money.CurrencyId, currency.CurrencyId);
        Assert.Equal(money.Value, currency.Amount);
    }

    [Fact]
    public async Task WhenEjectMoney()
    {
        var money = new Money(CurrencyCode.RUB, 23m);
        var id = await CreateWallet("Foo");

        await repository.EjectMoney(id, money);

        var portfolio = await repository.GetWallet(id);
        Assert.NotNull(portfolio);
        var currency = Assert.Single(portfolio.Currencies);
        Assert.Equal(money.CurrencyId, currency.CurrencyId);
        Assert.Equal(-money.Value, currency.Amount);
    }

    
    private async Task<WalletId> CreateWallet(string walletName)
    {
        var id = await repository.Create(walletName);
        await context.SaveChangesAsync();
        return id;
    }
}