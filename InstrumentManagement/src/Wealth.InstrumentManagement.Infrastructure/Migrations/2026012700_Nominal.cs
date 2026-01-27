using FluentMigrator;
using Wealth.InstrumentManagement.Domain.Instruments;

namespace Wealth.InstrumentManagement.Infrastructure.Migrations;

[Migration(2026012700)]
public class Nominal : Migration
{
    public override void Up()
    {
        Alter.Table("Bonds")
            .AddColumn("nominal_currency").AsByte().Nullable()
            .AddColumn("nominal_amount").AsCurrency().Nullable();
    }

    public override void Down()
    {
        Delete.Column("nominal_currency").FromTable("Bonds");
        Delete.Column("nominal_amount").FromTable("Bonds");
    }
}