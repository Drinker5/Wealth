using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wealth.CurrencyManagement.Domain.Currencies;
using Wealth.CurrencyManagement.Domain.ExchangeRates;

namespace Wealth.CurrencyManagement.Infrastructure.UnitOfWorks.EntityConfigurations;

internal class ExchangeRateConfiguration : IEntityTypeConfiguration<ExchangeRate>
{
    public void Configure(EntityTypeBuilder<ExchangeRate> builder)
    {
        builder.HasKey(x => new { BaseCurrencyId = x.BaseCurrency, TargetCurrencyId = x.TargetCurrency, x.ValidOnDate });

        builder.HasOne<Currency>()
            .WithMany()
            .HasForeignKey(i => i.BaseCurrency);

        builder.HasOne<Currency>()
            .WithMany()
            .HasForeignKey(i => i.TargetCurrency);

        builder.Property(x => x.BaseCurrency)
            .IsRequired();

        builder.Property(x => x.TargetCurrency)
            .IsRequired();

        builder.Property(x => x.ValidOnDate).HasColumnType("date").IsRequired();

        builder.Property(x => x.Rate).IsRequired();
        
        builder.HasNoDiscriminator();
    }
}