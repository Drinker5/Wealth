using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Wealth.DepositManagement.Domain.Deposits;

namespace Wealth.DepositManagement.Infrastructure.UnitOfWorks.EntityConfigurations.Converters;

public class DepositIdInMemoryValueGenerator : ValueGenerator<DepositId>
{
    private static int id;

    public override DepositId Next(EntityEntry entry)
    {
        return Interlocked.Increment(ref id);
    }

    public override bool GeneratesTemporaryValues => false;
}