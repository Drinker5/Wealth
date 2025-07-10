using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wealth.PortfolioManagement.Domain.Portfolios;
using Wealth.PortfolioManagement.Infrastructure.UnitOfWorks.EntityConfigurations.Converters;

namespace Wealth.PortfolioManagement.Infrastructure.UnitOfWorks.EntityConfigurations;

internal class PortfolioAssetConfiguration : IEntityTypeConfiguration<PortfolioAsset>
{
    public void Configure(EntityTypeBuilder<PortfolioAsset> builder)
    {
        // builder.Property<int>("PortfolioId");
        builder.Property<PortfolioId>("PortfolioId")
            .HasConversion<PortfolioIdConverter>();

        builder.HasKey("PortfolioId", nameof(PortfolioAsset.InstrumentId));

        builder.Property(x => x.InstrumentId)
            .HasConversion<InstrumentIdConverter>()
            .IsRequired();

        builder.Property(x => x.ISIN)
            .HasConversion<ISINConverter>()
            .HasMaxLength(12)
            .IsRequired();

        builder.Property(x => x.Quantity)
            .IsRequired();

        builder.HasOne<Portfolio>()
            .WithMany(i => i.Assets)
            .HasForeignKey("PortfolioId")
            .IsRequired();

        builder.HasNoDiscriminator();
    }
}