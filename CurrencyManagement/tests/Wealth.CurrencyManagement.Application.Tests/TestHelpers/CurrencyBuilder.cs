using Wealth.BuildingBlocks.Domain.Common;
using Wealth.CurrencyManagement.Domain.Currencies;

namespace Wealth.CurrencyManagement.Application.Tests.TestHelpers;

public class CurrencyBuilder
{
    private CurrencyId currencyId = "RUB";
    private string name = "Foo";
    private string symbol = "F";

    public Currency Build()
    {
        return Currency.Create(currencyId, name, symbol);
    }

    public CurrencyBuilder SetId(CurrencyId newCurrencyId)
    {
        this.currencyId = newCurrencyId;
        return this;
    }

    public CurrencyBuilder SetName(string newName)
    {
        this.name = newName;
        return this;
    }

    public CurrencyBuilder SetSymbol(string newSymbol)
    {
        symbol = newSymbol;
        return this;
    }
}