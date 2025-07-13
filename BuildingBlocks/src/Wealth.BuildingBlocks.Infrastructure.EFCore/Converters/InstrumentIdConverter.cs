using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.BuildingBlocks.Infrastructure.EFCore.Converters;

public class InstrumentIdConverter() : ValueConverter<InstrumentId, Guid>(v => v.Id, v => new InstrumentId(v));