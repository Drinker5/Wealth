using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.InstrumentManagement.Application.Instruments.Commands;

public class CreateBondCommand : ICommand<InstrumentId>
{
    public string Name { get; set; }
    public ISIN ISIN { get; set; }
}