using System.Runtime.InteropServices;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.InstrumentManagement.Application.Instruments.Models;

[StructLayout(LayoutKind.Auto)]
public readonly record struct InstrumentUIdPrice(InstrumentUId InstrumentUId, decimal Price);