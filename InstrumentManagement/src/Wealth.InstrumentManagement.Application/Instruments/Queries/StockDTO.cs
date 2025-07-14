using Wealth.InstrumentManagement.Domain.Instruments;

namespace Wealth.InstrumentManagement.Application.Instruments.Queries;

public record StockDTO(Guid Id, string ISIN, MoneyDTO Price, string Name, MoneyDTO Dividend)
    : InstrumentDTO(Id, ISIN, Price, Name)
{
    public static StockDTO From(StockInstrument instrument)
    {
        return new StockDTO(instrument.Id,
            instrument.ISIN,
            MoneyDTO.From(instrument.Price),
            instrument.Name,
            MoneyDTO.From(instrument.Dividend.ValuePerYear));
    }
    
    public override InstrumentType Type { get; } = InstrumentType.Stock;
}