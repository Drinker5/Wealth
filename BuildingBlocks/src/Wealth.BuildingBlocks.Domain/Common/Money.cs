namespace Wealth.BuildingBlocks.Domain.Common;

public record struct Money(CurrencyId CurrencyId, decimal Value) : IValueObject
{
    public static readonly Money Empty = new Money(0, 0);
    public static Money operator -(Money a)
    {
        return a with { Value = -a.Value };
    }

    public static Money operator -(Money a, Money b)
    {
        return a with { Value = a.Value - b.Value };
    }
    
    public static Money operator *(Money a, int b)
    {
        return a with { Value = a.Value * b };
    }
    
    public static Money operator /(Money a, int b)
    {
        return a with { Value = a.Value / b };
    }

    
    public static Money operator *(int b, Money a) => a * b;
}