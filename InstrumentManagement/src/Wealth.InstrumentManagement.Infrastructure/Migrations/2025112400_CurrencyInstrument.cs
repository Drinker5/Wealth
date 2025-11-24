using FluentMigrator;
using Wealth.InstrumentManagement.Domain.Instruments;

namespace Wealth.InstrumentManagement.Infrastructure.Migrations;

[Migration(2025112400)]
public class CurrencyInstrument : Migration
{
    public override void Up()
    {
        Create.Table("currencies")
            .WithColumn("id").AsInt32().PrimaryKey()
            .WithColumn("name").AsString(255).NotNullable()
            .WithColumn("figi").AsString(12).Unique()
            .WithColumn("price_currency").AsByte().Nullable()
            .WithColumn("price_amount").AsCustom("numeric").Nullable();
        
        Create.Sequence("currencies_hilo").IncrementBy(1);
    }

    public override void Down()
    {
        Delete.Table("currencies");
    }
}