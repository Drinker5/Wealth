using FluentMigrator;
using Wealth.InstrumentManagement.Domain.Instruments;

namespace Wealth.InstrumentManagement.Infrastructure.Migrations;

[Migration(2025071700)]
public class _2025071700_ProtoEvent : Migration
{
    public override void Up()
    {
        Delete.Column("Data").FromTable("OutboxMessages").InSchema("public");
        Alter.Table("OutboxMessages").InSchema("public")
            .AddColumn("Data").AsCustom("bytea").NotNullable();
    }

    public override void Down()
    {
        Delete.Column("Data").FromTable("OutboxMessages").InSchema("public");
        Alter.Table("OutboxMessages").InSchema("public")
            .AddColumn("Data").AsCustom("jsonb").NotNullable();
    }
}