using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.InMemory.ValueGeneration.Internal;
using Wealth.PortfolioManagement.Domain.Portfolios;

namespace Wealth.PortfolioManagement.Infrastructure.UnitOfWorks.EntityConfigurations.Converters;

public class PortfolioIdInMemoryIntegerValueGenerator : InMemoryIntegerValueGenerator<PortfolioId>
{
    private static int id;

    public PortfolioIdInMemoryIntegerValueGenerator(int propertyIndex) : base(propertyIndex)
    {
    }

    public override PortfolioId Next(EntityEntry entry)
    {
        return Interlocked.Increment(ref id);
    }
}