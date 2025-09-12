using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wealth.PortfolioManagement.Domain.Operations;
using Wealth.PortfolioManagement.Infrastructure.UnitOfWorks.EntityConfigurations.Converters;

namespace Wealth.PortfolioManagement.Infrastructure.UnitOfWorks.EntityConfigurations;

internal class BondTradeOperationConfiguration : IEntityTypeConfiguration<BondTradeOperation>
{
    public void Configure(EntityTypeBuilder<BondTradeOperation> builder)
    {
        builder.Property(x => x.PortfolioId)
            .IsRequired();

        builder.Property(x => x.BondId)
            .IsRequired();

        builder.Property(x => x.Quantity);
        builder.Property(x => x.Type);

        builder.ComplexProperty(y => y.Amount, z =>
        {
            z.Property(i => i.CurrencyId);
            z.Property(i => i.Amount);
        });
    }
}