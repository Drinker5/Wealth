using Wealth.BuildingBlocks.Domain;
using Wealth.PortfolioManagement.Domain.Portfolios.Events;

namespace Wealth.PortfolioManagement.Domain.Portfolios;

public class Portfolio : AggregateRoot
{
    public PortfolioId Id { get; private set; }
    public string Name { get; private set; }
    public ICollection<PortfolioAsset> Assets { get; } = [];
    public ICollection<PortfolioCurrency> Currencies { get; } = [];

    private Portfolio()
    {
    }

    public static Portfolio Create(string name) => Create(PortfolioId.New(), name);

    public static Portfolio Create(PortfolioId id, string name)
    {
        var portfolio = new Portfolio
        {
            Id = id,
            Name = name
        };

        portfolio.Apply(new PortfolioCreated(portfolio.Id, portfolio.Name));
        return portfolio;
    }

    public void AddCurrency(CurrencyId currencyId, decimal amount)
    {
        if (amount == 0)
            return;

        Apply(new CurrencyAdded(Id, currencyId, amount));
    }

    public void AddAsset(InstrumentId instrumentId, ISIN isin, int quantity)
    {
        if (quantity == 0)
            return;

        Apply(new AssetAdded(Id, instrumentId, isin, quantity));
    }

    private void When(PortfolioCreated @event)
    {
        Id = @event.PortfolioId;
        Name = @event.Name;
    }

    private void When(CurrencyAdded @event)
    {
        var existed = Currencies.SingleOrDefault(i => i.CurrencyId == @event.CurrencyId);
        if (existed != null)
        {
            existed.Amount += @event.Amount;
        }
        else
        {
            Currencies.Add(new PortfolioCurrency
            {
                CurrencyId = @event.CurrencyId,
                Amount = @event.Amount
            });
        }
    }

    private void When(AssetAdded @event)
    {
        var existed = Assets.SingleOrDefault(i => i.InstrumentId == @event.InstrumentId);
        if (existed != null)
        {
            existed.Quantity += @event.Quantity;
        }
        else
        {
            Assets.Add(new PortfolioAsset
            {
                InstrumentId = @event.InstrumentId,
                Quantity = @event.Quantity,
                ISIN = @event.ISIN,
            });
        }
    }

    public void Rename(string newName)
    {
        if (Name == newName)
            return;

        Apply(new PortfolioRenamed(Id, newName));
    }
}