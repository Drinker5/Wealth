using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Wealth.StrategyTracking.Domain.Strategies;

namespace Wealth.StrategyTracking.Infrastructure.UnitOfWorks.EntityConfigurations.Converters;

public class StrategyIdInMemoryValueGenerator : ValueGenerator<StrategyId>
{
    private static int id;

    public override StrategyId Next(EntityEntry entry)
    {
        return Interlocked.Increment(ref id);
    }

    public override bool GeneratesTemporaryValues => false;
}