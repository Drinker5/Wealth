using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wealth.PortfolioManagement.Domain.Operations;

namespace Wealth.PortfolioManagement.Infrastructure.UnitOfWorks.EntityConfigurations;

internal class BondAmortizationOperationConfiguration : IEntityTypeConfiguration<BondAmortizationOperation>
{
    public void Configure(EntityTypeBuilder<BondAmortizationOperation> builder)
    {
        builder.Property(x => x.PortfolioId)
            .IsRequired();

        builder.Property(x => x.BondId)
            .IsRequired();

        builder.ComplexProperty(y => y.Amount, z =>
        {
            z.Property(i => i.Currency);
            z.Property(i => i.Amount);
        });
    }
}