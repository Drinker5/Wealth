using FluentMigrator;
using Wealth.InstrumentManagement.Domain.Instruments;

namespace Wealth.InstrumentManagement.Infrastructure.Migrations;

[Migration(2025082600)]
public class Refactor : Migration
{
    public override void Up()
    {
        Delete.Table("Instruments");

        Create.Table("Bonds")
            .WithColumn($"Id").AsInt32().PrimaryKey()
            .WithColumn("Name").AsString(255).NotNullable()
            .WithColumn("ISIN").AsString(12).Unique()
            .WithColumn($"Price_CurrencyId").AsString(3).Nullable()
            .WithColumn($"Price_Amount").AsCustom("numeric").Nullable()
            .WithColumn($"Coupon_CurrencyId").AsString(3).Nullable()
            .WithColumn($"Coupon_Amount").AsCustom("numeric").Nullable();

        Create.Sequence("BondsHiLo").IncrementBy(1);

        Create.Table("Stocks")
            .WithColumn($"Id").AsInt32().PrimaryKey()
            .WithColumn("Name").AsString(255).NotNullable()
            .WithColumn("ISIN").AsString(12).Unique()
            .WithColumn($"Price_CurrencyId").AsString(3).Nullable()
            .WithColumn($"Price_Amount").AsCustom("numeric").Nullable()
            .WithColumn($"Dividend_CurrencyId").AsString(3).Nullable()
            .WithColumn($"Dividend_Amount").AsCustom("numeric").Nullable()
            .WithColumn("LotSize").AsInt32().NotNullable().WithDefaultValue(1);

        Create.Sequence("StocksHiLo");
    }

    public override void Down()
    {
        Delete.Table("Instruments");
    }
}