using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wealth.CurrencyManagement.Domain.Currencies;
using Wealth.CurrencyManagement.Domain.ExchangeRates;

namespace Wealth.CurrencyManagement.Infrastructure.UnitOfWorks.EntityConfigurations;

internal class ExchangeRateConfiguration : IEntityTypeConfiguration<ExchangeRate>
{
    public void Configure(EntityTypeBuilder<ExchangeRate> builder)
    {
        builder.HasKey(x => new { x.BaseCurrencyId, x.TargetCurrencyId, x.ValidOnDate });

        builder.HasOne<Currency>()
            .WithMany()
            .HasForeignKey(i => i.BaseCurrencyId);

        builder.HasOne<Currency>()
            .WithMany()
            .HasForeignKey(i => i.TargetCurrencyId);

        builder.Property(x => x.BaseCurrencyId)
            .HasConversion(new CurrencyIdConverter())
            .IsRequired();

        builder.Property(x => x.TargetCurrencyId)
            .HasConversion(new CurrencyIdConverter())
            .IsRequired();

        builder.Property(x => x.ValidOnDate).HasColumnType("date").IsRequired();

        builder.Property(x => x.Rate).IsRequired();
        
        builder.HasNoDiscriminator();
    }
}