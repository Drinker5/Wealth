namespace Wealth.InstrumentManagement.Domain;

public record CurrencyId(string Code)
{
    public static implicit operator string(CurrencyId id)
    {
        return id.Code;
    }
    
    public static implicit operator CurrencyId(string code)
    {
        return new CurrencyId(code);
    }
}