using Wealth.BuildingBlocks.Application;
using Wealth.InstrumentManagement.Domain.Instruments;

namespace Wealth.InstrumentManagement.Application.Instruments.Commands;

public class CreateBondCommand : ICommand
{
    public string Name { get; set; }
    public ISIN ISIN { get; set; }
}