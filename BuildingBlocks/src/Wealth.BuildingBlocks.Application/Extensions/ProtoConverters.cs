using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.BuildingBlocks;

public static class ProtoConverters
{
    public static InstrumentTypeProto ToProto(this InstrumentType instrumentType) =>
        instrumentType switch
        {
            InstrumentType.Stock => InstrumentTypeProto.Stock,
            InstrumentType.Bond => InstrumentTypeProto.Bond,
            InstrumentType.CurrencyAsset => InstrumentTypeProto.CurrencyAsset,
            InstrumentType.Currency => InstrumentTypeProto.Currency,
            _ => throw new ArgumentOutOfRangeException(nameof(instrumentType), instrumentType, null)
        };

    public static InstrumentType FromProto(this InstrumentTypeProto instrumentType) =>
        instrumentType switch
        {
            InstrumentTypeProto.Stock => InstrumentType.Stock,
            InstrumentTypeProto.Bond => InstrumentType.Bond,
            InstrumentTypeProto.CurrencyAsset => InstrumentType.CurrencyAsset,
            InstrumentTypeProto.Currency => InstrumentType.Currency,
            _ => throw new ArgumentOutOfRangeException(nameof(instrumentType), instrumentType, null)
        };
}