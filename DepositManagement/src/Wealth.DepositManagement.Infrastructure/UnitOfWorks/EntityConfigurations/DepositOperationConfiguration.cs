using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wealth.DepositManagement.Domain.DepositOperations;
using Wealth.DepositManagement.Domain.Deposits;
using Wealth.DepositManagement.Infrastructure.UnitOfWorks.EntityConfigurations.Converters;

namespace Wealth.DepositManagement.Infrastructure.UnitOfWorks.EntityConfigurations;

internal class DepositOperationConfiguration : IEntityTypeConfiguration<DepositOperation>
{
    public void Configure(EntityTypeBuilder<DepositOperation> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder.HasOne<Deposit>()
            .WithMany()
            .HasForeignKey(x => x.DepositId);

        builder.Property(x => x.DepositId)
            .IsRequired();

        builder.ComplexProperty(y => y.Money, z =>
        {
            z.Property(i => i.Currency).IsRequired().HasColumnName("Money_Currency");
            z.Property(i => i.Amount).IsRequired().HasColumnName("Money_Amount");
        });

        builder.Property(x => x.Type).IsRequired();

        builder.Property(x => x.Date).IsRequired();

        builder.UseTpcMappingStrategy();
    }
}