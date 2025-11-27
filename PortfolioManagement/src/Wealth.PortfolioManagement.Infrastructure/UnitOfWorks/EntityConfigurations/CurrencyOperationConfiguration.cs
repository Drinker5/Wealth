using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wealth.PortfolioManagement.Domain.Operations;
using Wealth.PortfolioManagement.Infrastructure.UnitOfWorks.EntityConfigurations.Converters;

namespace Wealth.PortfolioManagement.Infrastructure.UnitOfWorks.EntityConfigurations;

internal class CurrencyOperationConfiguration : IEntityTypeConfiguration<MoneyOperation>
{
    public void Configure(EntityTypeBuilder<MoneyOperation> builder)
    {
        builder.Property(x => x.PortfolioId)
            .IsRequired();

        builder.Property(x => x.Type);

        builder.ComplexProperty(y => y.Amount, z =>
        {
            z.Property(i => i.Currency);
            z.Property(i => i.Amount);
        });
    }
}