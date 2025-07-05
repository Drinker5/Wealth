using FluentMigrator;
using Wealth.InstrumentManagement.Domain.Instruments;

namespace Wealth.InstrumentManagement.Infrastructure.Migrations;

[Migration(2025070400)]
public class Init_2025070400 : Migration
{
    public override void Up()
    {
        Create.Table("Instruments")
            .WithColumn($"{nameof(Instrument.Id)}").AsGuid().PrimaryKey()
            .WithColumn(nameof(Instrument.Name)).AsString(255).NotNullable()
            .WithColumn(nameof(Instrument.ISIN)).AsString(12).Unique()
            .WithColumn(nameof(Instrument.Type)).AsInt32().NotNullable()
            .WithColumn($"{nameof(Instrument.Price)}_CurrencyId").AsString(3).Nullable()
            .WithColumn($"{nameof(Instrument.Price)}_Amount").AsDecimal().Nullable()
            .WithColumn($"{nameof(BondInstrument.Coupon)}_CurrencyId").AsString(3).Nullable()
            .WithColumn($"{nameof(BondInstrument.Coupon)}_Amount").AsDecimal().Nullable()
            .WithColumn($"{nameof(StockInstrument.Dividend)}_CurrencyId").AsString(3).Nullable()
            .WithColumn($"{nameof(StockInstrument.Dividend)}_Amount").AsDecimal().Nullable();
    }

    public override void Down()
    {
        Delete.Table("Instruments");
    }
}