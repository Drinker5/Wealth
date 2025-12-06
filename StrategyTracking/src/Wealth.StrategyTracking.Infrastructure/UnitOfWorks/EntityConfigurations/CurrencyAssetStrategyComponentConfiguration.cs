using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wealth.StrategyTracking.Domain.Strategies;

namespace Wealth.StrategyTracking.Infrastructure.UnitOfWorks.EntityConfigurations;

internal class CurrencyAssetStrategyComponentConfiguration : IEntityTypeConfiguration<CurrencyAssetStrategyComponent>
{
    public void Configure(EntityTypeBuilder<CurrencyAssetStrategyComponent> builder)
    {
    }
}