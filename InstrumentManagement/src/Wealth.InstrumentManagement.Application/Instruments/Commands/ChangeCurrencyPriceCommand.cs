using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.InstrumentManagement.Application.Instruments.Commands;

public class ChangeCurrencyPriceCommand : ICommand
{
    public CurrencyId CurrencyId { get; set; }
    public Money Price { get; set; }
}