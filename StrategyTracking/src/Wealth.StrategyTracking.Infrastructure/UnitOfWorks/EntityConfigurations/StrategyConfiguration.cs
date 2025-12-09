using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wealth.StrategyTracking.Domain.Strategies;

namespace Wealth.StrategyTracking.Infrastructure.UnitOfWorks.EntityConfigurations;

internal class StrategyConfiguration : IEntityTypeConfiguration<Strategy>
{
    public void Configure(EntityTypeBuilder<Strategy> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .UseHiLo("StrategyIdHiLo")
            .IsRequired();

        builder.Property(x => x.Name).HasMaxLength(255).IsRequired();

        builder.Property(x => x.FollowedStrategy)
            .HasDefaultValue(MasterStrategy.None)
            .IsRequired();

        builder.HasMany(i => i.Components)
            .WithOne()
            .HasForeignKey("StrategyId")
            .IsRequired();

        builder.HasNoDiscriminator();
    }
}