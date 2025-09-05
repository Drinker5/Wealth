using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.StrategyTracking.Domain.Strategies;

namespace Wealth.StrategyTracking.Infrastructure.UnitOfWorks.EntityConfigurations;

internal class StockStrategyComponentConfiguration : IEntityTypeConfiguration<StockStrategyComponent>
{
    public void Configure(EntityTypeBuilder<StockStrategyComponent> builder)
    {
    }
}
