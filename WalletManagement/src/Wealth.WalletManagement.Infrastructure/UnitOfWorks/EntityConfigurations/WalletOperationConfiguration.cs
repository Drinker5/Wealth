using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wealth.WalletManagement.Domain.WalletOperations;

namespace Wealth.WalletManagement.Infrastructure.UnitOfWorks.EntityConfigurations;

internal class WalletOperationConfiguration : IEntityTypeConfiguration<WalletOperation>
{
    public void Configure(EntityTypeBuilder<WalletOperation> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.WalletId)
            .IsRequired();

        builder.Property(x => x.Date)
            .IsRequired();

        builder.Property(x => x.OperationType)
            .IsRequired();

        builder.ComplexProperty(y => y.Amount, z =>
        {
            z.Property(i => i.CurrencyId)
                .HasMaxLength(3)
                .IsRequired();
                
            z.Property(i => i.Value)
                .IsRequired();
        });

        builder.HasNoDiscriminator();
    }
}