using System.Runtime.InteropServices;
using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.InstrumentManagement.Application.Instruments.Commands;

[StructLayout(LayoutKind.Auto)]
public record struct CreateBondCommand(
    string Name,
    ISIN Isin,
    FIGI Figi,
    InstrumentUId InstrumentUId) : ICommand<BondId>;