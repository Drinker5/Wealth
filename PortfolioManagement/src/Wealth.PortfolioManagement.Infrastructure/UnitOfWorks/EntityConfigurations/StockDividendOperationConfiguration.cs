using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wealth.PortfolioManagement.Domain.Operations;
using Wealth.PortfolioManagement.Infrastructure.UnitOfWorks.EntityConfigurations.Converters;

namespace Wealth.PortfolioManagement.Infrastructure.UnitOfWorks.EntityConfigurations;

internal class StockDividendOperationConfiguration : IEntityTypeConfiguration<StockDividendOperation>
{
    public void Configure(EntityTypeBuilder<StockDividendOperation> builder)
    {
        builder.Property(x => x.PortfolioId)
            .IsRequired();

        builder.Property(x => x.StockId)
            .IsRequired();

        builder.ComplexProperty(y => y.Amount, z =>
        {
            z.Property(i => i.CurrencyId);
            z.Property(i => i.Amount);
        });
    }
}