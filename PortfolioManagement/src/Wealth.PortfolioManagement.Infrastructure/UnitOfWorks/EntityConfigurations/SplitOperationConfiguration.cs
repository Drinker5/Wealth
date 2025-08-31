using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wealth.PortfolioManagement.Domain.Operations;

namespace Wealth.PortfolioManagement.Infrastructure.UnitOfWorks.EntityConfigurations;

internal class SplitOperationConfiguration : IEntityTypeConfiguration<SplitOperation>
{
    public void Configure(EntityTypeBuilder<SplitOperation> builder)
    {
        builder.ComplexProperty(x => x.Ratio, y =>
        {
            y.Property(z => z.Old);
            y.Property(z => z.New);
        });
    }
}