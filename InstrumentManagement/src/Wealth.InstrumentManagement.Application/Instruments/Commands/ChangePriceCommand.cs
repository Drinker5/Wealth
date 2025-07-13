using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.InstrumentManagement.Application.Instruments.Commands;

public class ChangePriceCommand : ICommand
{
    public InstrumentId Id { get; set; }
    public Money Price { get; set; }
}