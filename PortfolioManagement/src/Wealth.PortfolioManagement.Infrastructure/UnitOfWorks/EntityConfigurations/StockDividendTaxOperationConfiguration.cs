using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wealth.PortfolioManagement.Domain.Operations;

namespace Wealth.PortfolioManagement.Infrastructure.UnitOfWorks.EntityConfigurations;

internal class StockDividendTaxOperationConfiguration : IEntityTypeConfiguration<StockDividendTaxOperation>
{
    public void Configure(EntityTypeBuilder<StockDividendTaxOperation> builder)
    {
        builder.Property(x => x.PortfolioId)
            .IsRequired();

        builder.Property(x => x.StockId)
            .IsRequired();

        builder.ComplexProperty(y => y.Amount, z =>
        {
            z.Property(i => i.Currency).HasColumnName("Amount_Currency");
            z.Property(i => i.Amount).HasColumnName("Amount_Amount");
        });
    }
}