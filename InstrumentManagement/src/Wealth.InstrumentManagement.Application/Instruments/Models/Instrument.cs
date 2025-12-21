using System.Runtime.InteropServices;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.InstrumentManagement.Application.Instruments.Models;

[StructLayout(LayoutKind.Auto)]
public record struct Instrument(
    int Id,
    InstrumentId InstrumentId,
    InstrumentType Type);