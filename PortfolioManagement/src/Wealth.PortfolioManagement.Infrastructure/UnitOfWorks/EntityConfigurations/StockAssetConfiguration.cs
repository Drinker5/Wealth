using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.PortfolioManagement.Domain.Portfolios;
using Wealth.PortfolioManagement.Infrastructure.UnitOfWorks.EntityConfigurations.Converters;

namespace Wealth.PortfolioManagement.Infrastructure.UnitOfWorks.EntityConfigurations;

internal class StockAssetConfiguration : IEntityTypeConfiguration<StockAsset>
{
    public void Configure(EntityTypeBuilder<StockAsset> builder)
    {
        builder.Property<PortfolioId>("PortfolioId");

        builder.HasKey("PortfolioId", nameof(StockAsset.StockId));

        builder.Property(x => x.StockId)
            .IsRequired();

        builder.Property(x => x.Quantity)
            .IsRequired();

        builder.HasOne<Portfolio>()
            .WithMany(i => i.Stocks)
            .HasForeignKey("PortfolioId")
            .IsRequired();

        builder.HasNoDiscriminator();
    }
}