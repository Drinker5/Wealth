using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wealth.WalletManagement.Domain.Wallets;

namespace Wealth.WalletManagement.Infrastructure.UnitOfWorks.EntityConfigurations;

internal class WalletConfiguration : IEntityTypeConfiguration<Wallet>
{
    public void Configure(EntityTypeBuilder<Wallet> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .UseHiLo("WalletIdHiLo")
            .IsRequired();

        builder.Property(x => x.Name).HasMaxLength(255).IsRequired();

        builder.HasMany(i => i.Currencies)
            .WithOne()
            .HasForeignKey("WalletId")
            .IsRequired();

        builder.HasNoDiscriminator();
    }
}