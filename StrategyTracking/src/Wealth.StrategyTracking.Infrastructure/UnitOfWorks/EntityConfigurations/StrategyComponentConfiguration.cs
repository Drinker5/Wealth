using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wealth.StrategyTracking.Domain.Strategies;

namespace Wealth.StrategyTracking.Infrastructure.UnitOfWorks.EntityConfigurations;

internal class StrategyComponentConfiguration : IEntityTypeConfiguration<StockStrategyComponent>
{
    public void Configure(EntityTypeBuilder<StockStrategyComponent> builder)
    {
        builder.Property<StrategyId>("StrategyId");
        
        builder.HasKey("StrategyId", nameof(StockStrategyComponent.Id));

        builder.Property(x => x.StockId)
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