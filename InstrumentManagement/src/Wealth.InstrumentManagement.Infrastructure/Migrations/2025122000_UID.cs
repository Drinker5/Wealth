using FluentMigrator;
using Wealth.InstrumentManagement.Domain.Instruments;

namespace Wealth.InstrumentManagement.Infrastructure.Migrations;

[Migration(2025122000)]
public class UID : Migration
{
    public override void Up()
    {
        Alter.Table("Stocks")
            .AddColumn("instrument_id")
            .AsGuid()
            .NotNullable()
            .WithDefaultValue(Guid.Empty);
        
        Alter.Table("Bonds")
            .AddColumn("instrument_id")
            .AsGuid()
            .NotNullable()
            .WithDefaultValue(Guid.Empty);
        
        Alter.Table("currencies")
            .AddColumn("instrument_id")
            .AsGuid()
            .NotNullable()
            .WithDefaultValue(Guid.Empty);
    }

    public override void Down()
    {
        Delete.Column("instrument_id").FromTable("Stocks");
        Delete.Column("instrument_id").FromTable("Bonds");
        Delete.Column("instrument_id").FromTable("currencies");
    }
}