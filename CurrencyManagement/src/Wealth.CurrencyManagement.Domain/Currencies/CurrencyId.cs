using Wealth.BuildingBlocks.Domain;

namespace Wealth.CurrencyManagement.Domain.Currencies;

public record CurrencyId : IValueObject
{
    /// <summary>
    /// ISO 4217
    /// </summary>
    public string Code { get; private set; }

    /// <summary>
    /// </summary>
    /// <param name="code">ISO 4217</param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public CurrencyId(string code)
    {
        if (string.IsNullOrEmpty(code))
            throw new ArgumentNullException(nameof(code));

        if (code.Length != 3)
            throw new ArgumentException("Code must contain exactly 3 characters, ISO 4217");

        for (var i = 0; i < 3; i++)
        {
            if (!Char.IsUpper(code, i))
                throw new ArgumentException("Code must be in upper case letters");
        }

        Code = code;
    }

    public static implicit operator string(CurrencyId id)
    {
        return id.Code;
    }
    
    public static implicit operator CurrencyId(string code)
    {
        return new CurrencyId(code);
    }
    
    public override string ToString()
    {
        return Code;
    }
}