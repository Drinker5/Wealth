using FluentMigrator;
using Wealth.InstrumentManagement.Domain.Instruments;

namespace Wealth.InstrumentManagement.Infrastructure.Migrations;

[Migration(2026011300)]
public class Price : Migration
{
    public override void Up()
    {
        Alter.Table("Stocks")
            .AddColumn("price_updated_at")
            .AsDateTime()
            .Nullable();
        
        Alter.Table("Bonds")
            .AddColumn("price_updated_at")
            .AsDateTime()
            .Nullable();
        
        Alter.Table("currencies")
            .AddColumn("price_updated_at")
            .AsDateTime()
            .Nullable();
    }

    public override void Down()
    {
        Delete.Column("price_updated_at").FromTable("Stocks");
        Delete.Column("price_updated_at").FromTable("Bonds");
        Delete.Column("price_updated_at").FromTable("currencies");
    }
}