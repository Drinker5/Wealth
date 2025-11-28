using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.Aggregation.Application.Models;

public sealed record StockAggregationRaw(
    int StockId,
    CurrencyCode Currency,
    long Quantity,
    decimal TradeAmount,
    decimal MoneyAmount);