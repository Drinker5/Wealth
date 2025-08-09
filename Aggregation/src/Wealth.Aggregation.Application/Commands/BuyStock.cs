using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.Aggregation.Application.Commands;

public record BuyStock : ICommand
{
    public StockId StockId { get; set; }
    public Money TotalPrice { get; set; }
    public int Quantity { get; set; }
}
