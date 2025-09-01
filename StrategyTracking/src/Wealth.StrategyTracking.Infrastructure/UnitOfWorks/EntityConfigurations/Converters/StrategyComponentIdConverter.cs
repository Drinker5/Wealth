using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Wealth.StrategyTracking.Domain.Strategies;

namespace Wealth.StrategyTracking.Infrastructure.UnitOfWorks.EntityConfigurations.Converters;

public class StrategyComponentIdConverter() : ValueConverter<StrategyComponentId, Guid>(v => v.Value, v => new StrategyComponentId(v));