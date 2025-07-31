namespace Wealth.BuildingBlocks.Domain.Common;

public readonly record struct InstrumentId(Guid Id) : IIdentity
{
    public static InstrumentId New() => Guid.NewGuid();

    public static implicit operator Guid(InstrumentId id)
    {
        return id.Id;
    }
    
    public static implicit operator InstrumentId(Guid id)
    {
        return new InstrumentId(id);
    }

    public override string ToString()
    {
        return Id.ToString("N");
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}