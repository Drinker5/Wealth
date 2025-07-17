using FluentMigrator;
using Wealth.InstrumentManagement.Domain.Instruments;

namespace Wealth.InstrumentManagement.Infrastructure.Migrations;

[Migration(2025071601)]
public class _2025071601_StockLotSize : Migration
{
    public override void Up()
    {
        Alter.Table("Instruments").InSchema("public")
            .AddColumn("LotSize").AsInt32().NotNullable().WithDefaultValue(1);
    }

    public override void Down()
    {
        Delete.Column("LotSize").FromTable("Instruments").InSchema("public");
    }
}