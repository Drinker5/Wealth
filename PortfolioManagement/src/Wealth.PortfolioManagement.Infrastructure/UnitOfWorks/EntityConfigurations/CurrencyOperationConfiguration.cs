using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wealth.PortfolioManagement.Domain.Operations;
using Wealth.PortfolioManagement.Infrastructure.UnitOfWorks.EntityConfigurations.Converters;

namespace Wealth.PortfolioManagement.Infrastructure.UnitOfWorks.EntityConfigurations;

internal class CurrencyOperationConfiguration : IEntityTypeConfiguration<CurrencyOperation>
{
    public void Configure(EntityTypeBuilder<CurrencyOperation> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.PortfolioId)
            .HasConversion<PortfolioIdConverter>()
            .IsRequired();

        builder.Property(x => x.Type);

        builder.ComplexProperty(y => y.Money, z =>
        {
            z.Property(i => i.CurrencyId);
            z.Property(i => i.Amount);
        });
    }
}