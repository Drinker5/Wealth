using Wealth.BuildingBlocks.Domain.Common;
using Wealth.CurrencyManagement.Domain.Currencies;

namespace Wealth.CurrencyManagement.Domain.Tests.Entities;

[TestSubject(typeof(Currency))]
public class WhenCurrencyCreated
{
    private readonly CurrencyId currencyId = new CurrencyId("FOO");
    private readonly Currency currency;
    private readonly string name = "name";
    private readonly string symbol = "symbol";

    public WhenCurrencyCreated()
    {
        currency = Currency.Create(currencyId, name, symbol);
    }

    [Fact]
    public void ThenIdSet()
    {
        Assert.Equal(currencyId, currency.Id);
    }

    [Fact]
    public void ThenNameSet()
    {
        Assert.Equal("name", currency.Name);
    }

    [Fact]
    public void ThenSymbolSet()
    {
        Assert.Equal("symbol", currency.Symbol);
    }

    [Fact]
    public void ThenHasCurrencyCreatedEvent()
    {
        var ev = currency.HasEvent<CurrencyCreated>();
        
        Assert.Equal(currencyId, ev.CurrencyId);
        Assert.Equal(name, ev.Name);
        Assert.Equal(symbol, ev.Symbol);
    }
}