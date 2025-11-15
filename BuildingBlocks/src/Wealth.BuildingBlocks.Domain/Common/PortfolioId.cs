namespace Wealth.BuildingBlocks.Domain.Common;

public readonly record struct PortfolioId(int Value) : IIdentity
{
    public static PortfolioId New() => new(0);

    public static implicit operator int(PortfolioId id)
    {
        return id.Value;
    }

    public static implicit operator PortfolioId(int value)
    {
        return new PortfolioId(value);
    }

    public readonly override int GetHashCode()
    {
        return Value;
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}
