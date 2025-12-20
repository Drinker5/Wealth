namespace Wealth.BuildingBlocks.Domain.Common;

public readonly record struct InstrumentId(Guid Value) : IIdentity
{
    public static InstrumentId New() => new(Guid.NewGuid());

    public override string ToString()
    {
        return Value.ToString("D");
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
    
    public static implicit operator Guid(InstrumentId id)
    {
        return id.Value;
    }
    
    public static implicit operator InstrumentId(Guid value)
    {
        return new InstrumentId(value);
    }
    
    public static implicit operator InstrumentId(string value)
    {
        return new InstrumentId(Guid.Parse(value));
    }
}