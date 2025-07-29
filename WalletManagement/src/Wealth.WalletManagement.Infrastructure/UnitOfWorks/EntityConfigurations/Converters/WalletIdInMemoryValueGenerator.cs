using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Wealth.WalletManagement.Domain.Wallets;

namespace Wealth.WalletManagement.Infrastructure.UnitOfWorks.EntityConfigurations.Converters;

public class WalletIdInMemoryValueGenerator : ValueGenerator<WalletId>
{
    private static int id;

    public override WalletId Next(EntityEntry entry)
    {
        return Interlocked.Increment(ref id);
    }

    public override bool GeneratesTemporaryValues => false;
}