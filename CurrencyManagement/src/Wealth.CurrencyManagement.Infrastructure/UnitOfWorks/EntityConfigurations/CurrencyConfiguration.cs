using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wealth.CurrencyManagement.Domain.Currencies;

namespace Wealth.CurrencyManagement.Infrastructure.UnitOfWorks.EntityConfigurations;

internal class CurrencyConfiguration : IEntityTypeConfiguration<Currency>
{
    public void Configure(EntityTypeBuilder<Currency> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .IsRequired();

        builder.Property(x => x.Name);

        builder.Property(x => x.Symbol);

        builder.HasNoDiscriminator();
    }
}