using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.InstrumentManagement.Application.Instruments.Commands;

public class ChangeBondPriceCommand : ICommand
{
    public BondId BondId { get; set; }
    public Money Price { get; set; }
}