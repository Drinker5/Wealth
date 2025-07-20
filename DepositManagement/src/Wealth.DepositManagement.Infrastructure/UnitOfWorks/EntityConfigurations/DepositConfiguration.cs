using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.DepositManagement.Domain.DepositOperations;
using Wealth.DepositManagement.Domain.Deposits;
using Wealth.DepositManagement.Infrastructure.UnitOfWorks.EntityConfigurations.Converters;

namespace Wealth.DepositManagement.Infrastructure.UnitOfWorks.EntityConfigurations;

internal class DepositConfiguration : IEntityTypeConfiguration<Deposit>
{
    public void Configure(EntityTypeBuilder<Deposit> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .UseHiLo("DepositIdHiLo")
            .HasConversion<DepositIdConverter>()
            .IsRequired();

        builder.Property(x => x.Name).HasMaxLength(255).IsRequired();

        builder.Property(i => i.Yield)
            .HasConversion<YieldConverter>()
            .IsRequired();

        builder.OwnsOne(y => y.Investment, z =>
        {
            z.Property(i => i.CurrencyId).IsRequired();
            z.Property(i => i.Amount).IsRequired();
        });

        builder.Ignore(x => x.InterestPerYear);

        builder.HasMany<DepositOperation>()
            .WithOne()
            .HasForeignKey(x => x.DepositId)
            .IsRequired();

        builder.Ignore(x => x.DomainEvents);
        builder.HasNoDiscriminator();
    }
}