using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wealth.StrategyTracking.Domain.Strategies;

namespace Wealth.StrategyTracking.Infrastructure.UnitOfWorks.EntityConfigurations;

internal class StockStrategyComponentConfiguration : IEntityTypeConfiguration<StockStrategyComponent>
{
    public void Configure(EntityTypeBuilder<StockStrategyComponent> builder)
    {
        builder.Property(x => x.StockId)
            .IsRequired();
    }
}