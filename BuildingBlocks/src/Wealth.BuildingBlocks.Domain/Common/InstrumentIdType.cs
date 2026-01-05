namespace Wealth.BuildingBlocks.Domain.Common;

public readonly record struct InstrumentIdType(InstrumentId Id, InstrumentType Type)
{
    public InstrumentIdType(StockId stockId) : this(stockId.Value, InstrumentType.Stock)
    {
    }

    public InstrumentIdType(BondId bondId) : this(bondId.Value, InstrumentType.Bond)
    {
    }

    public InstrumentIdType(CurrencyId currencyId) : this(currencyId.Value, InstrumentType.CurrencyAsset)
    {
    }
}