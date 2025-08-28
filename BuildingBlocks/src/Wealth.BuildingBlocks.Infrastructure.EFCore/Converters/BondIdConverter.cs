using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.BuildingBlocks.Infrastructure.EFCore.Converters;

public class BondIdConverter() : ValueConverter<BondId, int>(v => v.Value, v => new BondId(v));