using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.InstrumentManagement.Application.Instruments.Commands;

public class CreateStockCommand : ICommand<StockId>
{
    public string Name { get; set; }
    public ISIN ISIN { get; set; }
}