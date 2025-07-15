using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.PortfolioManagement.Domain.Portfolios;
using Wealth.PortfolioManagement.Infrastructure.UnitOfWorks.EntityConfigurations.Converters;

namespace Wealth.PortfolioManagement.Infrastructure.UnitOfWorks.EntityConfigurations;

internal class PortfolioCurrencyConfiguration : IEntityTypeConfiguration<PortfolioCurrency>
{
    public void Configure(EntityTypeBuilder<PortfolioCurrency> builder)
    {
        // builder.Property<int>("PortfolioId");
        builder.Property<PortfolioId>("PortfolioId")
            .HasConversion<PortfolioIdConverter>();

        builder.HasKey("PortfolioId", nameof(PortfolioCurrency.CurrencyId));

        builder.Property(x => x.CurrencyId)
            .HasMaxLength(3)
            .IsRequired();

        builder.Property(x => x.Amount)
            .IsRequired();

        builder.HasOne<Portfolio>()
            .WithMany(i => i.Currencies)
            .HasForeignKey("PortfolioId")
            .IsRequired();

        builder.HasNoDiscriminator();
    }
}