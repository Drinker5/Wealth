using Wealth.BuildingBlocks.Domain;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.PortfolioManagement.Domain.Portfolios.Events;
using Wealth.PortfolioManagement.Domain.ValueObjects;

namespace Wealth.PortfolioManagement.Domain.Portfolios;

public class Portfolio : AggregateRoot
{
    public PortfolioId Id { get; private set; }
    public string Name { get; private set; }
    public ICollection<BondAsset> Bonds { get; } = [];
    public ICollection<StockAsset> Stocks { get; } = [];
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

    public void Deposit(Money money)
    {
        if (money.Amount == 0)
            return;

        Apply(new CurrencyDeposited(Id, money));
    }

    public void Withdraw(Money money)
    {
        if (money.Amount == 0)
            return;

        Apply(new CurrencyWithdrew(Id, money));
    }

    public void Buy(StockId instrumentId, Money totalPrice, int quantity)
    {
        if (quantity == 0)
            return;

        Apply(new StockBought(Id, instrumentId, totalPrice, quantity));
    }

    public void Sell(StockId instrumentId, Money price, int quantity)
    {
        if (quantity == 0)
            return;

        Apply(new StockSold(Id, instrumentId, price, quantity));
    }

    public void Buy(BondId instrumentId, Money totalPrice, int quantity)
    {
        if (quantity == 0)
            return;

        Apply(new BondBought(Id, instrumentId, totalPrice, quantity));
    }

    public void Sell(BondId instrumentId, Money price, int quantity)
    {
        if (quantity == 0)
            return;

        Apply(new BondSold(Id, instrumentId, price, quantity));
    }

    public void Amortization(BondId instrumentId, Money income)
    {
        if (Bonds.All(i => i.BondId != instrumentId))
            return;

        Apply(new AmortizationApplied(Id, instrumentId, income));
    }

    public void Coupon(BondId instrumentId, Money income)
    {
        if (Bonds.All(i => i.BondId != instrumentId))
            return;

        Apply(new CouponPaymentReceived(Id, instrumentId, income));
    }

    public void Dividend(StockId instrumentId, Money income)
    {
        if (Stocks.All(i => i.StockId != instrumentId))
            return;

        Apply(new DividendReceived(Id, instrumentId, income));
    }

    public void Tax(StockId instrumentId, Money expense)
    {
        if (Stocks.All(i => i.StockId != instrumentId))
            return;

        Apply(new StockOperationTaxPaid(Id, instrumentId, expense));
    }

    public void Tax(BondId instrumentId, Money expense)
    {
        if (Bonds.All(i => i.BondId != instrumentId))
            return;

        Apply(new BondOperationTaxPaid(Id, instrumentId, expense));
    }

    public void Split(StockId instrumentId, SplitRatio ratio)
    {
        if (ratio.Old == ratio.New)
            return;

        Apply(new StockSplit(Id, instrumentId, ratio));
    }

    public void Delist(StockId instrumentId)
    {
        if (Stocks.All(i => i.StockId != instrumentId))
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
        ChangeCurrencyAmount(@event.Money.CurrencyId, @event.Money.Amount);
    }

    private void When(CurrencyWithdrew @event)
    {
        ChangeCurrencyAmount(@event.Money.CurrencyId, -@event.Money.Amount);
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

    private void When(DividendReceived @event)
    {
        ChangeCurrencyAmount(@event.Income.CurrencyId, @event.Income.Amount);
    }

    private void When(CouponPaymentReceived @event)
    {
        ChangeCurrencyAmount(@event.Income.CurrencyId, @event.Income.Amount);
    }

    private void When(AmortizationApplied @event)
    {
        ChangeCurrencyAmount(@event.Income.CurrencyId, @event.Income.Amount);
    }

    private void When(StockOperationTaxPaid @event)
    {
        ChangeCurrencyAmount(@event.Expense.CurrencyId, -@event.Expense.Amount);
    }

    private void When(BondOperationTaxPaid @event)
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
        Stocks.Remove(asset);
    }

    private void ChangeAssetQuantity(StockId instrumentId, int quantity)
    {
        var asset = GetOrCreate(instrumentId);
        asset.Quantity += quantity;
        if (asset.Quantity == 0)
            Stocks.Remove(asset);
    }

    private void ChangeAssetQuantity(BondId instrumentId, int quantity)
    {
        var asset = GetOrCreate(instrumentId);
        asset.Quantity += quantity;
        if (asset.Quantity == 0)
            Bonds.Remove(asset);
    }

    private void ChangeCurrencyAmount(CurrencyId currencyId, decimal amount)
    {
        var currency = GetOrCreate(currencyId);
        currency.Amount += amount;
        if (currency.Amount == 0)
            Currencies.Remove(currency);
    }

    private StockAsset GetOrCreate(StockId instrumentId)
    {
        var existed = Stocks.SingleOrDefault(i => i.StockId == instrumentId);
        if (existed != null)
            return existed;

        var asset = new StockAsset
        {
            StockId = instrumentId,
            Quantity = 0,
        };
        Stocks.Add(asset);
        return asset;
    }

    private BondAsset GetOrCreate(BondId instrumentId)
    {
        var existed = Bonds.SingleOrDefault(i => i.BondId == instrumentId);
        if (existed != null)
            return existed;

        var asset = new BondAsset
        {
            BondId = instrumentId,
            Quantity = 0,
        };
        Bonds.Add(asset);
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