using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wealth.StrategyTracking.Domain.Strategies;

namespace Wealth.StrategyTracking.Infrastructure.UnitOfWorks.EntityConfigurations;

internal class CurrencyStrategyComponentConfiguration : IEntityTypeConfiguration<CurrencyStrategyComponent>
{
    public void Configure(EntityTypeBuilder<CurrencyStrategyComponent> builder)
    {
    }
}
