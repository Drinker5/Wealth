using System.Runtime.InteropServices;
using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement.Domain.Instruments;

namespace Wealth.InstrumentManagement.Application.Instruments.Commands;

[StructLayout(LayoutKind.Auto)]
public record struct CreateStockCommand(
    Ticker Ticker,
    string Name,
    ISIN Isin,
    FIGI Figi,
    InstrumentId InstrumentId,
    LotSize LotSize) : ICommand<StockId>;