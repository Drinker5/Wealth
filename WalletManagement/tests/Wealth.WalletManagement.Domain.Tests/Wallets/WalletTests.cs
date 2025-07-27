using Wealth.BuildingBlocks.Domain.Common;
using Wealth.WalletManagement.Domain.Tests.TestHelpers;
using Wealth.WalletManagement.Domain.Wallets;

namespace Wealth.WalletManagement.Domain.Tests.Wallets;

[TestSubject(typeof(Wallet))]
public class WalletTests
{
    private readonly Wallet wallet;
    private readonly string name = "Foo";

    public WalletTests()
    {
        wallet = Wallet.Create(name);
    }

    [Fact]
    public void WhenCreate()
    {
        Assert.Equal(0, wallet.Id.Id);
        Assert.Equal(name, wallet.Name);
        Assert.Empty(wallet.Currencies);
        var ev = wallet.HasEvent<WalletCreated>();
        Assert.Same(wallet, ev.Wallet);
    }

    [Fact]
    public void WhenInsert()
    {
        var money = new Money("FOO", 10);

        wallet.Insert(money);

        var currency = Assert.Single(wallet.Currencies);
        Assert.Equal(money.Amount, currency.Amount);
        Assert.Equal(money.CurrencyId, currency.CurrencyId);
        var ev = wallet.HasEvent<WalletMoneyInserted>();
        Assert.Equal(wallet.Id, ev.WalletId);
        Assert.Equal(money, ev.Money);
    }
    
    
    [Fact]
    public void WhenEject()
    {
        var money = new Money("FOO", 10);
        var eject = new Money("FOO", 3);
        wallet.Insert(money);
        
        wallet.Eject(eject);

        var currency = Assert.Single(wallet.Currencies);
        Assert.Equal(money.Amount - eject.Amount, currency.Amount);
        Assert.Equal(money.CurrencyId, currency.CurrencyId);
        var ev = wallet.HasEvent<WalletMoneyEjected>();
        Assert.Equal(wallet.Id, ev.WalletId);
        Assert.Equal(eject, ev.Money);
    }
    
    [Fact]
    public void WhenEjectSameMoney()
    {
        var money = new Money("FOO", 10);
        wallet.Insert(money);
        
        wallet.Eject(money);

        Assert.Empty(wallet.Currencies);
    }
}