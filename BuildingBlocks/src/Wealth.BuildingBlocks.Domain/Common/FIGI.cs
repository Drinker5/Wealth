namespace Wealth.BuildingBlocks.Domain.Common;

/// <summary>
/// FIGI (Financial Instrument Global Identifier) — глобальный идентификатор финансового инструмента.
/// Представляет собой 12-символьный код из латинских букв и цифр
/// https://developer.tbank.ru/invest/services/instruments/faq_instruments
/// </summary>
public readonly record struct FIGI : IValueObject
{
    public static FIGI Empty = new FIGI("000000000000"); 

    public string Value { get; }

    /// <param name="value">12-character alphanumeric code</param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public FIGI(string value)
    {
        if (string.IsNullOrEmpty(value))
            throw new ArgumentNullException(nameof(value));

        if (value.Length != 12)
            throw new ArgumentException("FIGI must be 12 characters long, ISO 6166");

        for (var i = 0; i < 12; i++)
        {
            if (!char.IsLetterOrDigit(value, i))
                throw new ArgumentException("FIGI must be alphanumeric");
        }

        Value = value;
    }

    public static implicit operator string(FIGI id)
    {
        return id.Value;
    }

    public static implicit operator FIGI(string value)
    {
        return new FIGI(value);
    }

    public override string ToString()
    {
        return Value;
    }
}