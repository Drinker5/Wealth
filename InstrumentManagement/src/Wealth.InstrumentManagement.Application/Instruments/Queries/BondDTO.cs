using Wealth.InstrumentManagement.Domain.Instruments;

namespace Wealth.InstrumentManagement.Application.Instruments.Queries;

public record BondDTO(Guid Id, string ISIN, MoneyDTO Price, string Name, MoneyDTO Coupon)
    : InstrumentDTO(Id, ISIN, Price, Name)
{
    public static BondDTO From(BondInstrument instrument)
    {
        return new BondDTO(instrument.Id,
            instrument.ISIN,
            MoneyDTO.From(instrument.Price),
            instrument.Name,
            MoneyDTO.From(instrument.Coupon.ValuePerYear));
    }

    public override InstrumentType Type { get; } = InstrumentType.Bond;
}