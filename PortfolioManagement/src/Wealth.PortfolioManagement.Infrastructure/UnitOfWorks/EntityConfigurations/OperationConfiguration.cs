using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wealth.PortfolioManagement.Domain.Operations;

namespace Wealth.PortfolioManagement.Infrastructure.UnitOfWorks.EntityConfigurations;

internal class OperationConfiguration : IEntityTypeConfiguration<Operation>
{
    public void Configure(EntityTypeBuilder<Operation> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder.HasDiscriminator<OperationType>("operation_type")
            .HasValue<CurrencyOperation>(OperationType.Currency)
            .HasValue<BondAmortizationOperation>(OperationType.BondAmortization)
            .HasValue<BondTradeOperation>(OperationType.BondTrade)
            .HasValue<StockTradeOperation>(OperationType.StockTrade)
            .HasValue<StockDelistOperation>(OperationType.Delist)
            .HasValue<StockSplitOperation>(OperationType.Split)
            .HasValue<BondCouponOperation>(OperationType.BondCoupon)
            .HasValue<StockDividendOperation>(OperationType.StockDividend)
            .HasValue<StockDividendTaxOperation>(OperationType.StockDividendTax);
    }

    private enum OperationType : byte
    {
        Currency = 0,
        BondAmortization = 1,
        BondTrade = 2,
        StockTrade = 3,
        Delist = 4,
        Split = 5,
        BondCoupon = 6,
        StockDividend = 7,
        StockDividendTax = 8,
    }
}