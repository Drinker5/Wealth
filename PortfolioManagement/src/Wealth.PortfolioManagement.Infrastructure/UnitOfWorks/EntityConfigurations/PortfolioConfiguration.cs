using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wealth.PortfolioManagement.Domain.Portfolios;
using Wealth.PortfolioManagement.Infrastructure.UnitOfWorks.EntityConfigurations.Converters;

namespace Wealth.PortfolioManagement.Infrastructure.UnitOfWorks.EntityConfigurations;

internal class PortfolioConfiguration : IEntityTypeConfiguration<Portfolio>
{
    public void Configure(EntityTypeBuilder<Portfolio> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .UseHiLo("PortfolioIdHiLo")
            .HasConversion<PortfolioIdConverter>()
            .IsRequired();

        builder.Property(x => x.Name).HasMaxLength(255).IsRequired();

        builder.HasMany(i => i.Currencies)
            .WithOne()
            .HasForeignKey("PortfolioId")
            .IsRequired();

        builder.HasMany(x => x.Assets)
            .WithOne()
            .HasForeignKey("PortfolioId")
            .IsRequired();

        builder.Ignore(x => x.DomainEvents);

        builder.HasNoDiscriminator();
    }
}