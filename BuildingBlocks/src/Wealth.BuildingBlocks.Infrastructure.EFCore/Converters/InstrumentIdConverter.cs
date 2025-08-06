using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.BuildingBlocks.Infrastructure.EFCore.Converters;

public class BondIdConverter() : ValueConverter<BondId, int>(v => v.Id, v => new BondId(v));