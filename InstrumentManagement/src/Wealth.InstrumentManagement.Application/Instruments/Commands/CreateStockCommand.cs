using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement.Domain.Instruments;

namespace Wealth.InstrumentManagement.Application.Instruments.Commands;

public sealed record CreateStockCommand : ICommand<StockId>
{
    public required string Name { get; init; }
    public required ISIN Isin { get; init; }
    public required FIGI Figi { get; init; }
    public required LotSize LotSize { get; init; }
}