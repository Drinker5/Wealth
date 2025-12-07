using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.StrategyTracking.Infrastructure.UnitOfWorks.EntityConfigurations.Converters;

public class StrategyIdConverter() : ValueConverter<StrategyId, int>(v => v.Value, v => new StrategyId(v));