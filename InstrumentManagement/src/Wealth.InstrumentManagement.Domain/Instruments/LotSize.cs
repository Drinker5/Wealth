namespace Wealth.InstrumentManagement.Domain.Instruments;

public readonly record struct LotSize(int Size)
{
    public override string ToString()
    {
        return Size.ToString();
    }

    public override int GetHashCode()
    {
        return Size.GetHashCode();
    }
    
    public static implicit operator int(LotSize value)
    {
        return value.Size;
    }
    
    public static implicit operator LotSize(int value)
    {
        return new LotSize(value);
    }
}