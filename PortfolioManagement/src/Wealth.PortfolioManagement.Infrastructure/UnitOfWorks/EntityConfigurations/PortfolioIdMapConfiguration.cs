using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wealth.PortfolioManagement.Application.Providers;

namespace Wealth.PortfolioManagement.Infrastructure.UnitOfWorks.EntityConfigurations;

internal class PortfolioIdMapConfiguration : IEntityTypeConfiguration<PortfolioIdMap>
{
    public void Configure(EntityTypeBuilder<PortfolioIdMap> builder)
    {
        builder.HasKey(i => i.AccountId);

        builder.Property(i => i.AccountId)
            .IsRequired();

        builder.Property(i => i.PortfolioId)
            .IsRequired();
    }
}