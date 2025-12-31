namespace Wealth.BuildingBlocks.Domain.Common;

public readonly record struct InstrumentId(int Value) : IIdentity
{
    public override string ToString()
    {
        return Value.ToString();
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public static implicit operator int(InstrumentId uId)
    {
        return uId.Value;
    }

    public static implicit operator InstrumentId(int value)
    {
        return new InstrumentId(value);
    }
}