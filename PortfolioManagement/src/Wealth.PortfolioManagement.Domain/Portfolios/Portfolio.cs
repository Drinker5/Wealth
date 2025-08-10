using Wealth.BuildingBlocks.Domain;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.PortfolioManagement.Domain.Portfolios.Events;
using Wealth.PortfolioManagement.Domain.ValueObjects;

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

    public static Portfolio Create(string name)
    {
        var portfolio = new Portfolio
        {
            Id = PortfolioId.New(),
            Name = name
        };

        portfolio.Apply(new PortfolioCreated(portfolio));
        return portfolio;
    }

    public void Deposit(CurrencyId currencyId, decimal amount)
    {
        if (amount == 0)
            return;

        Apply(new CurrencyDeposited(Id, currencyId, amount));
    }

    public void Withdraw(CurrencyId currencyId, decimal amount)
    {
        if (amount == 0)
            return;

        Apply(new CurrencyWithdrew(Id, currencyId, amount));
    }

    public void Buy(StockId instrumentId, Money totalPrice, int quantity)
    {
        if (quantity == 0)
            return;

        Apply(new AssetBought(Id, instrumentId, totalPrice, quantity));
    }

    public void Sell(StockId instrumentId, Money price, int quantity)
    {
        if (quantity == 0)
            return;

        Apply(new AssetSold(Id, instrumentId, price, quantity));
    }

    public void Income(InstrumentId instrumentId, Money income, IncomeType incomeType)
    {
        switch (incomeType)
        {
            case IncomeType.Amortization:
                Apply(new AmortizationApplied(Id, instrumentId, income));
                break;
            case IncomeType.Coupon:
                Apply(new CouponPaymentReceived(Id, instrumentId, income));
                break;
            case IncomeType.Dividend:
                Apply(new DividendReceived(Id, instrumentId, income));
                break;
        }
    }

    public void Expense(InstrumentId instrumentId, Money expense, ExpenseType expenseType)
    {
        switch (expenseType)
        {
            case ExpenseType.Tax:
                Apply(new TaxPaid(Id, instrumentId, expense));
                break;
        }
    }

    public void Split(InstrumentId instrumentId, SplitRatio ratio)
    {
        if (ratio.Old == ratio.New)
            return;

        Apply(new StockSplit(Id, instrumentId, ratio));
    }

    public void Delist(InstrumentId instrumentId)
    {
        if (Assets.All(i => i.InstrumentId != instrumentId))
            return;

        Apply(new StockDelisted(Id, instrumentId));
    }

    private void When(PortfolioCreated @event)
    {
        Id = @event.Portfolio.Id;
        Name = @event.Portfolio.Name;
    }

    private void When(CurrencyDeposited @event)
    {
        ChangeCurrencyAmount(@event.CurrencyId, @event.Amount);
    }

    private void When(CurrencyWithdrew @event)
    {
        ChangeCurrencyAmount(@event.CurrencyId, -@event.Amount);
    }

    private void When(StockBought @event)
    {
        ChangeAssetQuantity(@event.StockId, @event.Quantity);
        ChangeCurrencyAmount(@event.TotalPrice.CurrencyId, -@event.TotalPrice.Amount);
    }

    private void When(StockSold @event)
    {
        ChangeAssetQuantity(@event.StockId, -@event.Quantity);
        ChangeCurrencyAmount(@event.Price.CurrencyId, @event.Price.Amount);
    }

    private void When(BondBought @event)
    {
        ChangeAssetQuantity(@event.BondId, @event.Quantity);
        ChangeCurrencyAmount(@event.TotalPrice.CurrencyId, -@event.TotalPrice.Amount);
    }

    private void When(BondSold @event)
    {
        ChangeAssetQuantity(@event.BondId, -@event.Quantity);
        ChangeCurrencyAmount(@event.Price.CurrencyId, @event.Price.Amount);
    }

    private void When(IncomeApplied @event)
    {
        ChangeCurrencyAmount(@event.Income.CurrencyId, @event.Income.Amount);
    }

    private void When(TaxPaid @event)
    {
        ChangeCurrencyAmount(@event.Expense.CurrencyId, -@event.Expense.Amount);
    }

    private void When(StockSplit @event)
    {
        var asset = GetOrCreate(@event.StockId);
        asset.Quantity = @event.Ratio.Apply(asset.Quantity);
    }

    private void When(StockDelisted @event)
    {
        var asset = GetOrCreate(@event.StockId);
        Assets.Remove(asset);
    }

    private void ChangeAssetQuantity(InstrumentId instrumentId, int quantity)
    {
        var asset = GetOrCreate(instrumentId);
        asset.Quantity += quantity;
        if (asset.Quantity == 0)
            Assets.Remove(asset);
    }

    private void ChangeCurrencyAmount(CurrencyId currencyId, decimal amount)
    {
        var currency = GetOrCreate(currencyId);
        currency.Amount += amount;
        if (currency.Amount == 0)
            Currencies.Remove(currency);
    }

    private PortfolioAsset GetOrCreate(InstrumentId instrumentId)
    {
        var existed = Assets.SingleOrDefault(i => i.InstrumentId == instrumentId);
        if (existed != null)
            return existed;

        var asset = new PortfolioAsset
        {
            InstrumentId = instrumentId,
            Quantity = 0,
        };
        Assets.Add(asset);
        return asset;
    }

    private PortfolioCurrency GetOrCreate(CurrencyId currencyId)
    {
        var existed = Currencies.SingleOrDefault(i => i.CurrencyId == currencyId);
        if (existed != null)
            return existed;

        var currency = new PortfolioCurrency
        {
            CurrencyId = currencyId,
            Amount = 0,
        };

        Currencies.Add(currency);
        return currency;
    }

    public void Rename(string newName)
    {
        if (Name == newName)
            return;

        Apply(new PortfolioRenamed(Id, newName));
    }
}