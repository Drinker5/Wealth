namespace Wealth.InstrumentManagement.Domain.Instruments;

public readonly record struct LotSize(int Value)
{
    public static readonly LotSize One = new LotSize(1);

    public override string ToString()
    {
        return Value.ToString();
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
    
    public static implicit operator int(LotSize value)
    {
        return value.Value;
    }
    
    public static implicit operator LotSize(int value)
    {
        return new LotSize(value);
    }
}