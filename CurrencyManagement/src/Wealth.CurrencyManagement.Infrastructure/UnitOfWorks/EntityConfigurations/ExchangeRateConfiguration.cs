using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wealth.CurrencyManagement.Domain.ExchangeRates;

namespace Wealth.CurrencyManagement.Infrastructure.UnitOfWorks.EntityConfigurations;

internal class ExchangeRateConfiguration : IEntityTypeConfiguration<ExchangeRate>
{
    public void Configure(EntityTypeBuilder<ExchangeRate> builder)
    {
        builder.HasKey(x => new
        {
            BaseCurrency = x.BaseCurrency, 
            TargetCurrency = x.TargetCurrency, 
            ValidOnDate = x.ValidOnDate
        });

        builder.Property(x => x.BaseCurrency)
            .IsRequired();

        builder.Property(x => x.TargetCurrency)
            .IsRequired();

        builder.Property(x => x.ValidOnDate)
            .HasColumnType("date")
            .IsRequired();

        builder.Property(x => x.Rate)
            .IsRequired();
        
        builder.HasNoDiscriminator();
    }
}