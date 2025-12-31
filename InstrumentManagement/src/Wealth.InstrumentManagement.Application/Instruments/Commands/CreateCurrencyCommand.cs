using System.Runtime.InteropServices;
using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.InstrumentManagement.Application.Instruments.Commands;

[StructLayout(LayoutKind.Auto)]
public record struct CreateCurrencyCommand(
    string Name,
    FIGI Figi,
    InstrumentUId InstrumentUId) : ICommand<CurrencyId>;