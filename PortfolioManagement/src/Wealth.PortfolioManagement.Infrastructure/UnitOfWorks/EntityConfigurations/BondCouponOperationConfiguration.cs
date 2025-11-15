using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wealth.PortfolioManagement.Domain.Operations;

namespace Wealth.PortfolioManagement.Infrastructure.UnitOfWorks.EntityConfigurations;

internal class BondCouponOperationConfiguration : IEntityTypeConfiguration<BondCouponOperation>
{
    public void Configure(EntityTypeBuilder<BondCouponOperation> builder)
    {
        builder.Property(x => x.PortfolioId)
            .IsRequired();

        builder.Property(x => x.BondId)
            .IsRequired();

        builder.ComplexProperty(y => y.Amount, z =>
        {
            z.Property(i => i.CurrencyId);
            z.Property(i => i.Amount);
        });
    }
}