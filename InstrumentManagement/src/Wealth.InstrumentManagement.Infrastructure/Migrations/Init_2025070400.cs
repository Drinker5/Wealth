using FluentMigrator;

namespace Wealth.InstrumentManagement.Infrastructure.Migrations;

[Migration(2025070400)]
public class Init_2025070400 : Migration
{
    public override void Up()
    {
        Create.Table("Instruments")
            .WithColumn("Id").AsGuid().PrimaryKey()
            .WithColumn("Name").AsString(255).NotNullable()
            .WithColumn("ISIN").AsString(12).Unique()
            .WithColumn("InstrumentType").AsInt32().NotNullable()
            .WithColumn("Price_CurrencyId").AsString(3)
            .WithColumn("Price_Amount").AsDecimal()
            .WithColumn("Coupon_CurrencyId").AsString(3)
            .WithColumn("Coupon_Amount").AsDecimal()
            .WithColumn("Dividend_CurrencyId").AsString(3)
            .WithColumn("Dividend_Amount").AsDecimal();
    }

    public override void Down()
    {
        Delete.Table("Instruments");
    }
}