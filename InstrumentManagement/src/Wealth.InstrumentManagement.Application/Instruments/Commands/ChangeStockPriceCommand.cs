using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.InstrumentManagement.Application.Instruments.Commands;

public class ChangeStockPriceCommand : ICommand
{
    public StockId StockId { get; set; }
    public Money Price { get; set; }
}