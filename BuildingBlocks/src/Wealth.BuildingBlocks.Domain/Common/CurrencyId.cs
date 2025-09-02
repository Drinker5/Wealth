namespace Wealth.BuildingBlocks.Domain.Common;

public readonly record struct CurrencyId : IIdentity
{
    public CurrencyCode Value { get; }

    public CurrencyId(CurrencyCode value)
    {
        Value = value;
    }
    
    public CurrencyId(byte value)
    {
        Value = (CurrencyCode)value;
    }

    /// <summary>
    /// </summary>
    /// <param name="code">ISO 4217</param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public CurrencyId(string code)
    {
        if (string.IsNullOrEmpty(code))
            throw new ArgumentNullException(nameof(code));

        if (!Enum.TryParse(code, true, out CurrencyCode value))
            throw new ArgumentException($"Can't parse currency id {code}");

        Value = value;
    }

    public static implicit operator byte(CurrencyId id)
    {
        return (byte)id.Value;
    }

    public static implicit operator CurrencyId(byte code)
    {
        return new CurrencyId((CurrencyCode)code);
    }

    public static implicit operator CurrencyCode(CurrencyId id)
    {
        return id.Value;
    }

    public static implicit operator CurrencyId(CurrencyCode code)
    {
        return new CurrencyId(code);
    }
    
    public static implicit operator CurrencyId(string code)
    {
        return new CurrencyId(code);
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}