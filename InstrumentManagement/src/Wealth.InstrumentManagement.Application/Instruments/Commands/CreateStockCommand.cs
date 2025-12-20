using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement.Domain.Instruments;

namespace Wealth.InstrumentManagement.Application.Instruments.Commands;

public sealed record CreateStockCommand(
    string Ticker,
    string Name,
    ISIN Isin,
    FIGI Figi,
    InstrumentId InstrumentId,
    LotSize LotSize) : ICommand<StockId>;