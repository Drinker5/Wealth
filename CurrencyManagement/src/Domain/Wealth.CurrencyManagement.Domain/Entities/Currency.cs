using System.Runtime.CompilerServices;
using Wealth.CurrencyManagement.Domain.Interfaces;

namespace Wealth.CurrencyManagement.Domain.Entities;

public class Currency : AggregateRoot
{
    public CurrencyId Id { get; private set; }
    public string Name { get; private set; }
    public string Symbol { get; private set; }

    private Currency(CurrencyCreated e)
    {
        Id = e.CurrencyId;
        Name = e.Name;
        Symbol = e.Symbol;
    }

    public static Currency Create(CurrencyId id, string name, string symbol)
    {
        if (string.IsNullOrEmpty(symbol))
            throw new ArgumentNullException(nameof(symbol));

        if (string.IsNullOrEmpty(name))
            throw new ArgumentNullException(nameof(name));

        var currencyCreated = new CurrencyCreated(id, name, symbol);
        var currency = new Currency(currencyCreated);
        currency.Events.Add(currencyCreated);
        return currency;
    }

    public void Rename(string newName)
    {
        if (string.IsNullOrEmpty(newName))
            throw new ArgumentNullException(nameof(newName));

        if (Name == newName)
            return;


        var currencyRenamed = new CurrencyRenamed(Id, newName);
        When(currencyRenamed);
        Events.Add(currencyRenamed);
    }

    private void When(CurrencyRenamed e)
    {
        Name = e.NewName;
    }
}