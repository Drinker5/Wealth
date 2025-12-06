using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.StrategyTracking.Domain.Strategies;

namespace Wealth.StrategyTracking.Infrastructure.UnitOfWorks.EntityConfigurations;

internal class StrategyComponentConfiguration : IEntityTypeConfiguration<StrategyComponent>
{
    public void Configure(EntityTypeBuilder<StrategyComponent> builder)
    {
        builder.Property<StrategyId>("StrategyId");

        builder.Property<InstrumentType>("type")
            .IsRequired();

        builder.Property(i => i.Id)
            .IsRequired();
        
        builder.HasKey("StrategyId", "type", "Id");
        
        builder.HasDiscriminator<InstrumentType>("type")
            .HasValue<StockStrategyComponent>(InstrumentType.Stock)
            .HasValue<BondStrategyComponent>(InstrumentType.Bond)
            .HasValue<CurrencyAssetStrategyComponent>(InstrumentType.CurrencyAsset)
            .HasValue<CurrencyStrategyComponent>(InstrumentType.Currency);

        builder.Property(x => x.Weight)
            .IsRequired();

        builder.HasOne<Strategy>()
            .WithMany(i => i.Components)
            .HasForeignKey("StrategyId")
            .IsRequired();
    }
}