using Wealth.CurrencyManagement.Domain.Currency;

namespace Wealth.CurrencyManagement.Domain.Tests.Entities;

[TestSubject(typeof(Currency.Currency))]
public class WhenCurrencyRenamed
{
    private readonly CurrencyId currencyId = new CurrencyId("FOO");
    private readonly Currency.Currency currency;

    public WhenCurrencyRenamed()
    {
        currency = Currency.Currency.Create(currencyId, "name", "symbol");
        currency.Rename("new");
    }

    [Fact]
    public void WhenNameChanged()
    {
        Assert.Equal("new", currency.Name);
    }

    [Fact]
    public void ThenHasRenamedEvent()
    {
        var renamed = currency.HasEvent<CurrencyRenamed>();
        Assert.Equal(currencyId, renamed.CurrencyId);
        Assert.Equal("new", renamed.NewName);
    }
}