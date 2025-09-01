using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Wealth.StrategyTracking.Domain.Strategies;

namespace Wealth.StrategyTracking.Infrastructure.UnitOfWorks.EntityConfigurations.Converters;

public class StrategyIdConverter() : ValueConverter<StrategyId, int>(v => v.Value, v => new StrategyId(v));