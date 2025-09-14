using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.PortfolioManagement.Domain.Portfolios;

namespace Wealth.PortfolioManagement.Infrastructure.UnitOfWorks.EntityConfigurations;

internal class BondAssetConfiguration : IEntityTypeConfiguration<BondAsset>
{
    public void Configure(EntityTypeBuilder<BondAsset> builder)
    {
        builder.Property<PortfolioId>("PortfolioId");

        builder.HasKey("PortfolioId", nameof(BondAsset.BondId));

        builder.Property(x => x.BondId)
            .IsRequired();

        builder.Property(x => x.Quantity)
            .IsRequired();

        builder.HasOne<Portfolio>()
            .WithMany(i => i.Bonds)
            .HasForeignKey("PortfolioId")
            .IsRequired();

        builder.HasNoDiscriminator();
    }
}