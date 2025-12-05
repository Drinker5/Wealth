using FluentMigrator;
using Wealth.InstrumentManagement.Domain.Instruments;

namespace Wealth.InstrumentManagement.Infrastructure.Migrations;

[Migration(2025120400)]
public class StockIndex : Migration
{
    public override void Up()
    {
        Alter.Table("Stocks")
            .AddColumn("index").AsString().NotNullable();
    }

    public override void Down()
    {
        Delete.Column("index").FromTable("Stocks");
    }
}