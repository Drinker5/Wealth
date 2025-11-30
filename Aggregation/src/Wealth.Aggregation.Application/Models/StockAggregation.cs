using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.Aggregation.Application.Models;

public sealed record StockAggregation(
    string Index,
    string Name,
    decimal Weight,
    decimal WeightSkipped,
    Money UnitPrice,
    Money UnitDividendPerYear,
    int LotSize,
    long Quantity,
    Money TotalInvenstment,
    Money MoneyAmount)
{
    public Money LotPrice => UnitPrice * LotSize;

    public int ToBuy(decimal Target) => Convert.ToInt32(Math.Round(Target * WeightSkipped / UnitPrice.Amount, 0)) % LotSize;

    public Money Value => UnitPrice * Quantity;

    public decimal CompletePercent(decimal Target) => (decimal)Quantity / ToBuy(Target);

    public Money DividendPerYear => UnitDividendPerYear * Quantity;

    public Money Balance => Value - TotalInvenstment + MoneyAmount;
}