using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wealth.PortfolioManagement.Domain.Operations;

namespace Wealth.PortfolioManagement.Infrastructure.UnitOfWorks.EntityConfigurations;

internal class StockSplitOperationConfiguration : IEntityTypeConfiguration<StockSplitOperation>
{
    public void Configure(EntityTypeBuilder<StockSplitOperation> builder)
    {
        builder.ComplexProperty(x => x.Ratio, y =>
        {
            y.Property(z => z.Old);
            y.Property(z => z.New);
        });

        builder.Property(x => x.StockId)
            .IsRequired();
    }
}