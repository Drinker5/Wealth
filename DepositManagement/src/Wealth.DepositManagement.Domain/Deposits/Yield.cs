using Wealth.BuildingBlocks.Domain;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.DepositManagement.Domain.Deposits;

public record struct Yield(decimal PercentPerYear) : IValueObject
{
    public override string ToString()
    {
        return $"{PercentPerYear * 100:0.00}%";
    }

    public override int GetHashCode()
    {
        return PercentPerYear.GetHashCode();
    }
    
    public static Money operator *(Money a, Yield b)
    {
        return a with { Amount = a.Amount * b.PercentPerYear };
    }
}