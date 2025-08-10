using FluentMigrator;
using Wealth.InstrumentManagement.Domain.Instruments;

namespace Wealth.InstrumentManagement.Infrastructure.Migrations;

[Migration(2025071800)]
public class ChangeNumeric : Migration
{
    public override void Up()
    {
        Alter.Column($"Price_Amount")
            .OnTable("Instruments").InSchema("public")
            .AsCustom("numeric");
        Alter.Column($"Coupon_Amount")
            .OnTable("Instruments").InSchema("public")
            .AsCustom("numeric");
        Alter.Column($"Dividend_Amount")
            .OnTable("Instruments").InSchema("public")
            .AsCustom("numeric");;
    }

    public override void Down()
    {
        Alter.Column($"Price_Amount")
            .OnTable("Instruments").InSchema("public")
            .AsDecimal();
        Alter.Column($"Coupon_Amount")
            .OnTable("Instruments").InSchema("public")
            .AsDecimal();
        Alter.Column($"Dividend_Amount")
            .OnTable("Instruments").InSchema("public")
            .AsDecimal();
    }
}