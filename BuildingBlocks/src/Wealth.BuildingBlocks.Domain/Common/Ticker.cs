namespace Wealth.BuildingBlocks.Domain.Common;

public readonly record struct Ticker : IValueObject
{
    public static Ticker Empty = new Ticker("0");

    public const int MaxLength = 10;
    public string Value { get; }

    /// <param name="value">10-character letters</param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public Ticker(string value)
    {
        if (string.IsNullOrEmpty(value))
            throw new ArgumentNullException(nameof(value));

        if (value.Length > MaxLength)
            throw new ArgumentException($"Ticker must be {MaxLength} characters long");

        Value = value;
    }

    public static implicit operator string(Ticker id)
    {
        return id.Value;
    }

    public static implicit operator Ticker(string value)
    {
        return new Ticker(value);
    }

    public override string ToString()
    {
        return Value;
    }
}