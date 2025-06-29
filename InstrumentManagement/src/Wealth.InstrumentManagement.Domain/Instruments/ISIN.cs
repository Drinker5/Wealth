using Wealth.BuildingBlocks.Domain;

namespace Wealth.InstrumentManagement.Domain.Instruments;

/// <summary>
/// An ISIN, or International Securities Identification Number, is a 12-character
/// alphanumeric code that uniquely identifies a specific security
/// </summary>
public record ISIN : IValueObject
{
    public static ISIN Empty = new ISIN("000000000000"); 

    public string Value { get; }

    /// <param name="value">12-character alphanumeric code</param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public ISIN(string value)
    {
        if (string.IsNullOrEmpty(value))
            throw new ArgumentNullException(nameof(value));

        if (value.Length != 12)
            throw new ArgumentException("ISIN must be 12 characters long, ISO 6166");

        for (var i = 0; i < 12; i++)
        {
            if (!Char.IsLetterOrDigit(value, i))
                throw new ArgumentException("ISIN must be alphanumeric");
        }

        Value = value;
    }

    public static implicit operator string(ISIN id)
    {
        return id.Value;
    }

    public static implicit operator ISIN(string value)
    {
        return new ISIN(value);
    }

    public override string ToString()
    {
        return Value;
    }
}