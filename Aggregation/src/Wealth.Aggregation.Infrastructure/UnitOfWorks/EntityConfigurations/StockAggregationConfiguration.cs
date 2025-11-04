using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wealth.Aggregation.Domain;

namespace Wealth.Aggregation.Infrastructure.UnitOfWorks.EntityConfigurations;

internal class StockAggregationConfiguration : IEntityTypeConfiguration<StockAggregation>
{
    public void Configure(EntityTypeBuilder<StockAggregation> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id);

        builder.ComplexProperty(y => y.StockPrice, z =>
        {
            z.Property(i => i.CurrencyId);
            z.Property(i => i.Value);
        });
        
        builder.ComplexProperty(y => y.DividendPerYear, z =>
        {
            z.Property(i => i.CurrencyId);
            z.Property(i => i.Value);
        });

        builder.Property(x => x.LotSize);

        builder.ComplexProperty(y => y.TotalInvestments, z =>
        {
            z.Property(i => i.CurrencyId);
            z.Property(i => i.Value);
        });
        
        builder.ComplexProperty(y => y.TotalDividends, z =>
        {
            z.Property(i => i.CurrencyId);
            z.Property(i => i.Value);
        });

        builder.Ignore(x => x.CurrentDividendValue);
        builder.Ignore(x => x.CurrentValue);

        builder.HasNoDiscriminator();
    }
}