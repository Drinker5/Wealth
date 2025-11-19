using FluentMigrator;
using Wealth.InstrumentManagement.Domain.Instruments;

namespace Wealth.InstrumentManagement.Infrastructure.Migrations;

[Migration(2025111900)]
public class Currency : Migration
{
    public override void Up()
    {
        Delete.Column("Price_CurrencyId").FromTable("Bonds");
        Delete.Column("Coupon_CurrencyId").FromTable("Bonds");
        
        Delete.Column("Price_CurrencyId").FromTable("Stocks");
        Delete.Column("Dividend_CurrencyId").FromTable("Stocks");
        
        Alter.Table("Bonds")
            .AddColumn("Price_Currency").AsByte().Nullable()
            .AddColumn("Coupon_Currency").AsByte().Nullable();

        Alter.Table("Stocks")
            .AddColumn("Price_Currency").AsByte().Nullable()
            .AddColumn("Dividend_Currency").AsByte().Nullable();
    }

    public override void Down()
    {
        Delete.Column("Price_Currency").FromTable("Bonds");
        Delete.Column("Coupon_Currency").FromTable("Bonds");

        Delete.Column("Price_Currency").FromTable("Stocks");
        Delete.Column("Dividend_Currency").FromTable("Stocks");

        Alter.Table("Bonds")
            .AddColumn("Price_CurrencyId").AsByte().Nullable()
            .AddColumn("Coupon_CurrencyId").AsByte().Nullable();

        Alter.Table("Stocks")
            .AddColumn("Price_CurrencyId").AsByte().Nullable()
            .AddColumn("Dividend_CurrencyId").AsByte().Nullable();
    }
}