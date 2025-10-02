using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wealth.PortfolioManagement.Domain.Operations;

namespace Wealth.PortfolioManagement.Infrastructure.UnitOfWorks.EntityConfigurations;

internal class BondTradeOperationConfiguration : IEntityTypeConfiguration<BondTradeOperation>
{
    public void Configure(EntityTypeBuilder<BondTradeOperation> builder)
    {
        builder.Property(x => x.BondId)
            .IsRequired();
    }
}