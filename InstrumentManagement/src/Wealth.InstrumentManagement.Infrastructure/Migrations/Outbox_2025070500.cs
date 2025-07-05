using FluentMigrator;
using Wealth.InstrumentManagement.Domain.Instruments;

namespace Wealth.InstrumentManagement.Infrastructure.Migrations;

[Migration(2025070500)]
public class Outbox_2025070500 : Migration
{
    public override void Up()
    {
        Create.Table("OutboxMessages").InSchema("public")
            .WithColumn("Id").AsGuid().PrimaryKey()
            .WithColumn("Type").AsString(255).NotNullable()
            .WithColumn("Data").AsCustom("jsonb").NotNullable()
            .WithColumn("OccurredOn").AsDateTimeOffset().NotNullable()
            .WithColumn("ProcessedOn").AsDateTimeOffset().Nullable()
            .WithColumn("Error").AsString().Nullable();
        
        Create.Index("IX_OutboxMessages_ProcessedOn")
            .OnTable("OutboxMessages").InSchema("public")
            .OnColumn("ProcessedOn").Ascending()
            .WithOptions().NonClustered();
    }

    public override void Down()
    {
        Delete.Table("OutboxMessages").InSchema("public");
    }
}