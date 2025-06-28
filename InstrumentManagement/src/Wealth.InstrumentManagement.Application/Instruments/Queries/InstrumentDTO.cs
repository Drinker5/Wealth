using Wealth.InstrumentManagement.Domain.Instruments;

namespace Wealth.InstrumentManagement.Application.Instruments.Queries;

public abstract record InstrumentDTO(
    Guid Id,
    string ISIN,
    MoneyDTO Price,
    string Name)
{
    public static InstrumentDTO From(Instrument instrument)
    {
        switch (instrument)
        {
            case BondInstrument bondInstrument:
                return BondDTO.From(bondInstrument);
            case StockInstrument stockInstrument:
                return StockDTO.From(stockInstrument);
            default:
                throw new ArgumentException("Unknown instrument type");
        }
    }
}