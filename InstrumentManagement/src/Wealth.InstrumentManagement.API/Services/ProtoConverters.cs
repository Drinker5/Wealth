namespace Wealth.InstrumentManagement.API.Services;

public static class ProtoConverters
{
    public static InstrumentType ToProto(this Domain.Instruments.InstrumentType type)
    {
        return type switch
        {
            Domain.Instruments.InstrumentType.Bond => InstrumentType.ItBond,
            Domain.Instruments.InstrumentType.Stock => InstrumentType.ItStock,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }
}