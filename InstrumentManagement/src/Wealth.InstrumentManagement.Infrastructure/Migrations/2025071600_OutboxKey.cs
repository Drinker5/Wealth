using FluentMigrator;
using Wealth.InstrumentManagement.Domain.Instruments;

namespace Wealth.InstrumentManagement.Infrastructure.Migrations;

[Migration(2025071600)]
public class _2025071600_OutboxKey : Migration
{
    public override void Up()
    {
        Alter.Table("OutboxMessages").InSchema("public")
            .AddColumn("Key").AsString(255).NotNullable().WithDefaultValue("undefined");
    }

    public override void Down()
    {
        Delete.Column("Key").FromTable("OutboxMessages").InSchema("public");
    }
}