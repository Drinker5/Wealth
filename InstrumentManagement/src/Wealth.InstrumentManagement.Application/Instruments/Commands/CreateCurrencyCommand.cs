using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.InstrumentManagement.Application.Instruments.Commands;

public sealed record CreateCurrencyCommand : ICommand<CurrencyId>
{
    public required string Name { get; init; }
    public required FIGI Figi { get; init; }
    public required InstrumentId InstrumentId { get; init; }
}