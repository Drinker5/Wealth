namespace Wealth.InstrumentManagement;

public partial class DecimalProto
{
    private const decimal NanoFactor = 1_000_000_000;

    public DecimalProto(long units, int nanos)
    {
        Units = units;
        Nanos = nanos;
    }

    public static implicit operator decimal(DecimalProto grpcDecimal)
    {
        return grpcDecimal.Units + grpcDecimal.Nanos / NanoFactor;
    }

    public static implicit operator DecimalProto(decimal value)
    {
        var units = decimal.ToInt64(value);
        var nanos = decimal.ToInt32((value - units) * NanoFactor);
        return new DecimalProto(units, nanos);
    }
}