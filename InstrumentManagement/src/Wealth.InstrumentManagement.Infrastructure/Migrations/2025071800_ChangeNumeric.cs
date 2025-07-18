using FluentMigrator;
using Wealth.InstrumentManagement.Domain.Instruments;

namespace Wealth.InstrumentManagement.Infrastructure.Migrations;

[Migration(2025071800)]
public class ChangeNumeric : Migration
{
    public override void Up()
    {
        Alter.Column($"{nameof(Instrument.Price)}_Amount")
            .OnTable("Instruments").InSchema("public")
            .AsCustom("numeric");
        Alter.Column($"{nameof(BondInstrument.Coupon)}_Amount")
            .OnTable("Instruments").InSchema("public")
            .AsCustom("numeric");
        Alter.Column($"{nameof(StockInstrument.Dividend)}_Amount")
            .OnTable("Instruments").InSchema("public")
            .AsCustom("numeric");;
    }

    public override void Down()
    {
        Alter.Column($"{nameof(Instrument.Price)}_Amount")
            .OnTable("Instruments").InSchema("public")
            .AsDecimal();
        Alter.Column($"{nameof(BondInstrument.Coupon)}_Amount")
            .OnTable("Instruments").InSchema("public")
            .AsDecimal();
        Alter.Column($"{nameof(StockInstrument.Dividend)}_Amount")
            .OnTable("Instruments").InSchema("public")
            .AsDecimal();
    }
}