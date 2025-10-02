using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.InstrumentManagement.Application.Instruments.Commands;

public sealed record CreateBondCommand : ICommand<BondId>
{
    public required string Name { get; init; }
    public required ISIN Isin { get; init; }
    public required FIGI Figi { get; init; }
}