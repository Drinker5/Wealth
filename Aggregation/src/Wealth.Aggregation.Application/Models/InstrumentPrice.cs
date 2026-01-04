using System.Runtime.InteropServices;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.Aggregation.Application.Models;

[StructLayout(LayoutKind.Auto)]
public record struct InstrumentPrice(InstrumentIdType InstrumentIdType, Money Price);