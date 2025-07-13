using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement.Domain.Instruments;

namespace Wealth.InstrumentManagement.Application.Instruments.Commands;

public class CreateStockCommand : ICommand<InstrumentId>
{
    public string Name { get; set; }
    public ISIN ISIN { get; set; }
}