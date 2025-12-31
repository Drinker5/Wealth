namespace Wealth.BuildingBlocks.Domain.Common;

public readonly record struct InstrumentUId(Guid Value) : IIdentity
{
    public static InstrumentUId New() => new(Guid.NewGuid());
    public static InstrumentUId From(string value) => new(Guid.Parse(value));

    public override string ToString()
    {
        return Value.ToString("D");
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
    
    public static implicit operator Guid(InstrumentUId uId)
    {
        return uId.Value;
    }
    
    public static implicit operator InstrumentUId(Guid value)
    {
        return new InstrumentUId(value);
    }
    
    public static implicit operator InstrumentUId(string value)
    {
        return new InstrumentUId(Guid.Parse(value));
    }
}