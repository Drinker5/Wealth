using Wealth.BuildingBlocks.Domain;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.WalletManagement.Domain.Wallets;

public class Wallet : AggregateRoot
{
    public WalletId Id { get; private set; }
    public string Name { get; private set; }
    public ICollection<WalletCurrency> Currencies { get; } = [];

    private Wallet()
    {
    }

    public static Wallet Create(string name)
    { 
        var wallet = new Wallet
        {
            Id = WalletId.New(),
            Name = name
        };

        wallet.Apply(new WalletCreated(wallet));
        return wallet;
    }

    public void Insert(Money money)
    {
        if (money.Value == 0)
            return;

        Apply(new WalletMoneyInserted(Id, money));
    }

    public void Eject(Money money)
    {
        if (money.Value == 0)
            return;

        Apply(new WalletMoneyEjected(Id, money));
    }

    public void Rename(string newName)
    {
        if (Name == newName)
            return;

        Apply(new WalletRenamed(Id, newName));
    }
    
    private void When(WalletCreated @event)
    {
        Id = @event.Wallet.Id;
        Name = @event.Wallet.Name;
    }

    private void When(WalletMoneyInserted @event)
    {
        ChangeCurrencyAmount(@event.Money);
    }

    private void When(WalletMoneyEjected @event)
    {
        ChangeCurrencyAmount(-@event.Money);
    }

    private void When(WalletRenamed @event)
    {
        Name = @event.NewName;
    }
    
    private void ChangeCurrencyAmount(Money money)
    {
        var currency = GetOrCreate(money.CurrencyId);
        currency.Amount += money.Value;
        if (currency.Amount == 0)
            Currencies.Remove(currency);
    }
    
    private WalletCurrency GetOrCreate(CurrencyId currencyId)
    {
        var existed = Currencies.SingleOrDefault(i => i.CurrencyId == currencyId);
        if (existed != null)
            return existed;

        var currency = new WalletCurrency
        {
            CurrencyId = currencyId,
            Amount = 0,
        };

        Currencies.Add(currency);
        return currency;
    }
}