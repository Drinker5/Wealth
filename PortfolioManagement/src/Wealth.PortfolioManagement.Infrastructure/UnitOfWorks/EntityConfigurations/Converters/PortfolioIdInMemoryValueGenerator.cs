using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.PortfolioManagement.Infrastructure.UnitOfWorks.EntityConfigurations.Converters;

public class PortfolioIdInMemoryValueGenerator : ValueGenerator<PortfolioId>
{
    private static int id;

    public override PortfolioId Next(EntityEntry entry)
    {
        return Interlocked.Increment(ref id);
    }

    public override bool GeneratesTemporaryValues => false;
}