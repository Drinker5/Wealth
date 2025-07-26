using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wealth.PortfolioManagement.Domain.Operations;
using Wealth.PortfolioManagement.Infrastructure.UnitOfWorks.EntityConfigurations.Converters;

namespace Wealth.PortfolioManagement.Infrastructure.UnitOfWorks.EntityConfigurations;

internal class InstrumentOperationConfiguration : IEntityTypeConfiguration<InstrumentOperation>
{
    public void Configure(EntityTypeBuilder<InstrumentOperation> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.InstrumentId)
            .IsRequired();

        builder.Property(x => x.Date)
            .IsRequired();

        builder.UseTpcMappingStrategy();
    }
}

internal class DelistOperationConfiguration : IEntityTypeConfiguration<DelistOperation>
{
    public void Configure(EntityTypeBuilder<DelistOperation> builder)
    {
        builder.ToTable("DelistOperations");
    }
}

internal class SplitOperationConfiguration : IEntityTypeConfiguration<SplitOperation>
{
    public void Configure(EntityTypeBuilder<SplitOperation> builder)
    {
        builder.ToTable("SplitOperations");

        builder.OwnsOne(x => x.Ratio, y =>
        {
            y.Property(z => z.Old);
            y.Property(z => z.New);
        });
    }
}

internal class CashOperationConfiguration : IEntityTypeConfiguration<CashOperation>
{
    public void Configure(EntityTypeBuilder<CashOperation> builder)
    {
        builder.ToTable("CashOperations");

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

internal class TradeOperationConfiguration : IEntityTypeConfiguration<TradeOperation>
{
    public void Configure(EntityTypeBuilder<TradeOperation> builder)
    {
        builder.ToTable("TradeOperations");

        builder.Property(x => x.PortfolioId)
            .HasConversion<PortfolioIdConverter>()
            .IsRequired();

        builder.Property(x => x.Quantity);
        builder.Property(x => x.Type);

        builder.ComplexProperty(y => y.Money, z =>
        {
            z.Property(i => i.CurrencyId);
            z.Property(i => i.Amount);
        });
    }
}