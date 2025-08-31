using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wealth.PortfolioManagement.Domain.Operations;

namespace Wealth.PortfolioManagement.Infrastructure.UnitOfWorks.EntityConfigurations;

internal class StockTradeOperationConfiguration : IEntityTypeConfiguration<StockTradeOperation>
{
    public void Configure(EntityTypeBuilder<StockTradeOperation> builder)
    {
        builder.Property(x => x.PortfolioId)
            .IsRequired();

        builder.Property(x => x.StockId)
            .IsRequired();

        builder.Property(x => x.Quantity);
        builder.Property(x => x.Type);

        builder.ComplexProperty(y => y.Money, z =>
        {
            z.Property(i => i.CurrencyId);
            z.Property(i => i.Amount);
        });
    }
}