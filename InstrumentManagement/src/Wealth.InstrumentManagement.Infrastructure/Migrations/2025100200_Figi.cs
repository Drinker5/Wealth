using FluentMigrator;
using Wealth.InstrumentManagement.Domain.Instruments;

namespace Wealth.InstrumentManagement.Infrastructure.Migrations;

[Migration(2025100200)]
public class Figi : Migration
{
    public override void Up()
    {
        Alter.Table("Bonds")
            .AddColumn("FIGI")
            .AsString(12)
            .Unique();

        Alter.Table("Stocks")
            .AddColumn("FIGI")
            .AsString(12)
            .Unique();
    }

    public override void Down()
    {
        Delete.Column("FIGI").FromTable("Bonds");
        
        Delete.Column("FIGI").FromTable("Stocks");
    }
}