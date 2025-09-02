using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wealth.StrategyTracking.Domain.Strategies;

namespace Wealth.StrategyTracking.Infrastructure.UnitOfWorks.EntityConfigurations;

internal class BondStrategyComponentConfiguration : IEntityTypeConfiguration<BondStrategyComponent>
{
    public void Configure(EntityTypeBuilder<BondStrategyComponent> builder)
    {
        builder.Property(b => b.BondId)
            .HasField("id")
            .IsRequired();
    }
}