using Wealth.BuildingBlocks.Domain;

namespace Wealth.PortfolioManagement.Domain.ValueObjects;

public readonly record struct SplitRatio : IValueObject
{
    public SplitRatio(int old, int @new)
    {
        if (old < 0 || @new < 0)
            throw new ArgumentOutOfRangeException();

        this.Old = old;
        this.New = @new;
    }

    public bool IsReverse => New > Old;
    public int Old { get; init; }
    public int New { get; init; }

    public int Apply(int amount)
    {
        if (amount % Old != 0)
            throw new InvalidOperationException("Cannot apply split");

        return amount / Old * New;
    }
}