using Wealth.BuildingBlocks.Application;
using Wealth.InstrumentManagement.Domain;
using Wealth.InstrumentManagement.Domain.Instruments;

namespace Wealth.InstrumentManagement.Application.Instruments.Commands;

public class ChangePriceCommand : ICommand
{
    public InstrumentId Id { get; set; }
    public Money Price { get; set; }
}