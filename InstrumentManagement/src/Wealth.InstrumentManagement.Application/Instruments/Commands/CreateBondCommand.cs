using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.InstrumentManagement.Application.Instruments.Commands;

public class CreateBondCommand : ICommand<BondId>
{
    public string Name { get; set; }
    public ISIN ISIN { get; set; }
}