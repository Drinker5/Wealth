using Wealth.BuildingBlocks.Application;
using Wealth.InstrumentManagement.Domain.Instruments;

namespace Wealth.InstrumentManagement.Application.Instruments.Commands;

public class ChangeDividendCommand : ICommand
{
    public InstrumentId Id { get; set; }
    public Dividend Dividend { get; set; }
}