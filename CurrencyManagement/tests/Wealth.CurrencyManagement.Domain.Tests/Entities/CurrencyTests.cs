using Wealth.CurrencyManagement.Domain.Entities;

namespace Wealth.CurrencyManagement.Domain.Tests.Entities;

[TestSubject(typeof(Currency))]
public class CurrencyTests
{
    private readonly CurrencyId currencyId = new CurrencyId("FOO");
    
    [Fact]
    public void CreateCurrency_NameIsNotSet()
    {
        Assert.Throws<ArgumentNullException>(() => Currency.Create(currencyId, string.Empty, "bar"));
    }
    
    [Fact]
    public void CreateCurrency_SymbolIsNotSet()
    {
        Assert.Throws<ArgumentNullException>(() => Currency.Create(currencyId, "name", string.Empty));
    }
    
    
}