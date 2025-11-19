using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wealth.PortfolioManagement.Domain.Operations;

namespace Wealth.PortfolioManagement.Infrastructure.UnitOfWorks.EntityConfigurations;

internal class TradeOperationConfiguration : IEntityTypeConfiguration<TradeOperation>
{
    public void Configure(EntityTypeBuilder<TradeOperation> builder)
    {
        builder.Property(x => x.PortfolioId)
            .IsRequired();

        builder.Property(x => x.Quantity);
        builder.Property(x => x.Type);

        builder.ComplexProperty(y => y.Amount, z =>
        {
            z.Property(i => i.Currency);
            z.Property(i => i.Amount);
        });
    }
}