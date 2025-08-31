using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wealth.PortfolioManagement.Domain.Operations;

namespace Wealth.PortfolioManagement.Infrastructure.UnitOfWorks.EntityConfigurations;

internal class DelistOperationConfiguration : IEntityTypeConfiguration<DelistOperation>
{
    public void Configure(EntityTypeBuilder<DelistOperation> builder)
    {
        builder.ToTable("DelistOperations");
    }
}