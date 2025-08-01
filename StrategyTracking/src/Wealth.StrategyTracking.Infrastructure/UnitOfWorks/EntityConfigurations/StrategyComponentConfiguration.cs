using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wealth.StrategyTracking.Domain.Strategies;

namespace Wealth.StrategyTracking.Infrastructure.UnitOfWorks.EntityConfigurations;

internal class StrategyComponentConfiguration : IEntityTypeConfiguration<StrategyComponent>
{
    public void Configure(EntityTypeBuilder<StrategyComponent> builder)
    {
        builder.Property<StrategyId>("StrategyId");
        
        builder.HasKey("StrategyId", nameof(StrategyComponent.InstrumentId));

        builder.Property(x => x.InstrumentId)
            .IsRequired();

        builder.Property(x => x.Weight)
            .IsRequired();

        builder.HasOne<Strategy>()
            .WithMany(i => i.Components)
            .HasForeignKey("StrategyId")
            .IsRequired();

        builder.HasNoDiscriminator();
    }
}