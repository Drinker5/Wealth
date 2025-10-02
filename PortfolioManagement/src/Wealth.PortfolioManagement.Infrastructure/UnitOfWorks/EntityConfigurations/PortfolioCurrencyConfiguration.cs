using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.BuildingBlocks.Infrastructure.EFCore.Converters;
using Wealth.PortfolioManagement.Domain.Portfolios;

namespace Wealth.PortfolioManagement.Infrastructure.UnitOfWorks.EntityConfigurations;

internal class PortfolioCurrencyConfiguration : IEntityTypeConfiguration<PortfolioCurrency>
{
    public void Configure(EntityTypeBuilder<PortfolioCurrency> builder)
    {
        builder.Property<PortfolioId>("PortfolioId");

        builder.HasKey("PortfolioId", nameof(PortfolioCurrency.CurrencyId));

        builder.Property(x => x.CurrencyId)
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