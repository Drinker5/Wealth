using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wealth.PortfolioManagement.Domain.Portfolios;

namespace Wealth.PortfolioManagement.Infrastructure.UnitOfWorks.EntityConfigurations;

internal class PortfolioConfiguration : IEntityTypeConfiguration<Portfolio>
{
    public void Configure(EntityTypeBuilder<Portfolio> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .UseHiLo("PortfolioIdHiLo")
            .IsRequired();

        builder.Property(x => x.Name).HasMaxLength(255).IsRequired();

        builder.HasMany(i => i.Currencies)
            .WithOne()
            .HasForeignKey("PortfolioId")
            .IsRequired();

        builder.HasMany(x => x.Stocks)
            .WithOne()
            .HasForeignKey("PortfolioId")
            .IsRequired();

        builder.HasMany(x => x.Bonds)
            .WithOne()
            .HasForeignKey("PortfolioId")
            .IsRequired();
        
        builder.HasNoDiscriminator();
    }
}