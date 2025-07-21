namespace Wealth.BuildingBlocks.Domain.Common;

public readonly record struct DepositId(int Id) : IIdentity
{
    public static DepositId New() => new(0);

    public override string ToString()
    {
        return Id.ToString();
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
    
    public static implicit operator int(DepositId value)
    {
        return value.Id;
    }
    
    public static implicit operator DepositId(int value)
    {
        return new DepositId(value);
    }
}