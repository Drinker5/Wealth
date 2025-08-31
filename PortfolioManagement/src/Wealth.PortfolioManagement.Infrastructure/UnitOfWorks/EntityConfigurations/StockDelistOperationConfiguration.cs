using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wealth.PortfolioManagement.Domain.Operations;

namespace Wealth.PortfolioManagement.Infrastructure.UnitOfWorks.EntityConfigurations;

internal class StockDelistOperationConfiguration : IEntityTypeConfiguration<StockDelistOperation>
{
    public void Configure(EntityTypeBuilder<StockDelistOperation> builder)
    {
        builder.Property(x => x.StockId)
            .IsRequired();
    }
}