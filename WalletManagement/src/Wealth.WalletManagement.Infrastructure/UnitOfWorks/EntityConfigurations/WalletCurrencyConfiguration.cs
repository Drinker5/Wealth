using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.WalletManagement.Domain.Wallets;

namespace Wealth.WalletManagement.Infrastructure.UnitOfWorks.EntityConfigurations;

internal class WalletCurrencyConfiguration : IEntityTypeConfiguration<WalletCurrency>
{
    public void Configure(EntityTypeBuilder<WalletCurrency> builder)
    {
        builder.Property<WalletId>("WalletId");

        builder.HasKey("WalletId", nameof(WalletCurrency.Currency));

        builder.Property(x => x.Currency)
            .HasMaxLength(3)
            .IsRequired();

        builder.Property(x => x.Amount)
            .IsRequired();

        builder.HasOne<Wallet>()
            .WithMany(i => i.Currencies)
            .HasForeignKey("WalletId")
            .IsRequired();

        builder.HasNoDiscriminator();
    }
}