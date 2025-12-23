using FluentMigrator;
using Wealth.InstrumentManagement.Domain.Instruments;

namespace Wealth.InstrumentManagement.Infrastructure.Migrations;

[Migration(2025120400)]
public class StockTicker : Migration
{
    public override void Up()
    {
        Alter.Table("Stocks")
            .AddColumn("ticker")
            .AsString(10)
            .NotNullable()
            .WithDefaultValue("0");
    }

    public override void Down()
    {
        Delete.Column("ticker").FromTable("Stocks");
    }
}