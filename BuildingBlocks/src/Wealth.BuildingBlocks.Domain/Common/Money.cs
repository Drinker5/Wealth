namespace Wealth.BuildingBlocks.Domain.Common;

public record struct Money(CurrencyId CurrencyId, decimal Amount) : IValueObject
{
    public static readonly Money Empty = new Money("AAA", 0);
    public static Money operator -(Money a)
    {
        return a with { Amount = -a.Amount };
    }

    public static Money operator -(Money a, Money b)
    {
        return a with { Amount = a.Amount - b.Amount };
    }
    
    public static Money operator *(Money a, int b)
    {
        return a with { Amount = a.Amount * b };
    }
    
    public static Money operator /(Money a, int b)
    {
        return a with { Amount = a.Amount / b };
    }

    
    public static Money operator *(int b, Money a) => a * b;
}