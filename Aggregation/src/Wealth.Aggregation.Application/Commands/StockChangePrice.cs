using Wealth.BuildingBlocks;
using Wealth.BuildingBlocks.Application;

namespace Wealth.Aggregation.Application.Commands;

public class StockChangePrice : ICommand
{
    public InstrumentIdProto InstrumentId { get; set; }
    public MoneyProto NewPrice { get; set; }
}