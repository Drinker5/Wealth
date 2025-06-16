using Wealth.CurrencyManagement.Domain.Interfaces;

namespace Wealth.CurrencyManagement.Domain.Currency;

public class Currency : AggregateRoot
{
    public CurrencyId Id { get; private set; }
    public string Name { get; private set; }
    public string Symbol { get; private set; }

    private Currency()
    {
    }

    public static Currency Create(CurrencyId id, string name, string symbol)
    {
        if (string.IsNullOrEmpty(symbol))
            throw new ArgumentNullException(nameof(symbol));

        if (string.IsNullOrEmpty(name))
            throw new ArgumentNullException(nameof(name));

        var currency = new Currency();
        currency.Apply(new CurrencyCreated(id, name, symbol));
        return currency;
    }

    public void Rename(string newName)
    {
        if (string.IsNullOrEmpty(newName))
            throw new ArgumentNullException(nameof(newName));

        if (Name == newName)
            return;


        Apply(new CurrencyRenamed(Id, newName));
    }

    private void When(CurrencyCreated e)
    {
        Id = e.CurrencyId;
        Name = e.Name;
        Symbol = e.Symbol;
    }

    private void When(CurrencyRenamed e)
    {
        Name = e.NewName;
    }
}