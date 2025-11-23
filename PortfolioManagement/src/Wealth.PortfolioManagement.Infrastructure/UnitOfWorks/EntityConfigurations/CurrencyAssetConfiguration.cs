using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.PortfolioManagement.Domain.Portfolios;

namespace Wealth.PortfolioManagement.Infrastructure.UnitOfWorks.EntityConfigurations;

internal class CurrencyAssetConfiguration : IEntityTypeConfiguration<CurrencyAsset>
{
    public void Configure(EntityTypeBuilder<CurrencyAsset> builder)
    {
        builder.Property<PortfolioId>("PortfolioId");

        builder.HasKey("PortfolioId", nameof(CurrencyAsset.CurrencyId));

        builder.Property(x => x.CurrencyId)
            .IsRequired();

        builder.Property(x => x.Quantity)
            .IsRequired();

        builder.HasOne<Portfolio>()
            .WithMany(i => i.CurrencyAssets)
            .HasForeignKey("PortfolioId")
            .IsRequired();

        builder.HasNoDiscriminator();
    }
}