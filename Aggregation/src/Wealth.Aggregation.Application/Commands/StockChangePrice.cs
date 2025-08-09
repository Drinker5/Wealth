using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.Aggregation.Application.Commands;

public class StockChangePrice : ICommand
{
    public StockId StockId { get; set; }
    public Money NewPrice { get; set; }
}