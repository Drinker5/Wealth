using FluentMigrator;
using Wealth.InstrumentManagement.Domain.Instruments;

namespace Wealth.InstrumentManagement.Infrastructure.Migrations;

[Migration(2025070400)]
public class _2025070400_Init : Migration
{
    public override void Up()
    {
        Create.Table("Instruments")
            .WithColumn($"Id").AsGuid().PrimaryKey()
            .WithColumn("Name").AsString(255).NotNullable()
            .WithColumn("ISIN").AsString(12).Unique()
            .WithColumn("Type").AsInt32().NotNullable()
            .WithColumn($"Price_CurrencyId").AsString(3).Nullable()
            .WithColumn($"Price_Amount").AsDecimal().Nullable()
            .WithColumn($"Coupon_CurrencyId").AsString(3).Nullable()
            .WithColumn($"Coupon_Amount").AsDecimal().Nullable()
            .WithColumn($"Dividend_CurrencyId").AsString(3).Nullable()
            .WithColumn($"Dividend_Amount").AsDecimal().Nullable();
    }

    public override void Down()
    {
        Delete.Table("Instruments");
    }
}