using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wealth.StrategyTracking.Domain.Strategies;

namespace Wealth.StrategyTracking.Infrastructure.UnitOfWorks.EntityConfigurations;

internal class StrategyComponentConfiguration : IEntityTypeConfiguration<StrategyComponent>
{
    public void Configure(EntityTypeBuilder<StrategyComponent> builder)
    {
        builder.ToTable("StrategyComponents");
        
        builder.Property<StrategyId>("StrategyId");
        builder.HasKey("StrategyId", nameof(StrategyComponent.Id));

        builder.HasDiscriminator<StrategyComponentType>("type")
            .HasValue<StockStrategyComponent>(StrategyComponentType.Stock)
            .HasValue<BondStrategyComponent>(StrategyComponentType.Bond)
            .HasValue<CurrencyStrategyComponent>(StrategyComponentType.Currency);

        builder.Property(x => x.Id)
            .IsRequired();

        builder.Property(x => x.Weight)
            .IsRequired();

        builder.HasOne<Strategy>()
            .WithMany(i => i.Components)
            .HasForeignKey("StrategyId")
            .IsRequired();
    }
}