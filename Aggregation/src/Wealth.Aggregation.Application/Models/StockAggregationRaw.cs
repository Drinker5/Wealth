using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.Aggregation.Application.Models;

public sealed record StockAggregationRaw(
    InstrumentIdType InstrumentIdType,
    CurrencyCode Currency,
    long Quantity,
    decimal TradeAmount,
    decimal MoneyAmount, 
    decimal Price);