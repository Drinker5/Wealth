using Wealth.BuildingBlocks.Domain;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.Aggregation.Domain;

public class StockAggregation : AggregateRoot
{
    public InstrumentId Id { get; init; }
    public string Name { get; private set; }

    public Money Price { get; private set; }
    public Money DividendPerYear { get; private set; }
    public int LotSize { get; private set; }
    public int Quantity { get; private set; }

    public Money CurrentValue => Price * Quantity;
    public Money CurrentDividendValue => DividendPerYear * Quantity;

    public Money TotalInvestments { get; private set; }
    public Money TotalDividends { get; private set; }

    private StockAggregation()
    {
    }

    public StockAggregation(
        InstrumentId id,
        string name,
        Money price,
        Money dividendPerYear,
        int lotSize)
    {
        Id = id;
        Name = name;
        Price = price;
        DividendPerYear = dividendPerYear;
        LotSize = lotSize;
        Quantity = 0;
        TotalInvestments = price with { Amount = 0 };
        TotalDividends = price with { Amount = 0 };
    }

    public void ChangeName(string name)
    {
        Name = name;
    }

    public void ChangePrice(Money price)
    {
        Price = price;
    }

    public void ChangeDividendPerYear(Money dividendPerYear)
    {
        DividendPerYear = dividendPerYear;
    }

    public void ChangeLotSize(int lotSize)
    {
        LotSize = lotSize;
    }

    public void Buy(int quantity, Money investment)
    {
        Quantity += quantity;
        AddInvestment(investment);
    }

    public void Sell(int quantity, Money profit)
    {
        Quantity -= quantity;
        AddInvestment(-profit);
    }

    public void AddDividend(Money dividend)
    {
        if (dividend.CurrencyId != TotalDividends.CurrencyId)
            throw new ArgumentException("Different Currencies are not supported");

        TotalDividends = dividend with { Amount = TotalDividends.Amount + dividend.Amount };
    }

    private void AddInvestment(Money investment)
    {
        if (investment.CurrencyId != TotalInvestments.CurrencyId)
            throw new ArgumentException("Different Currencies are not supported");

        TotalInvestments = investment with { Amount = TotalInvestments.Amount + investment.Amount };
    }
}