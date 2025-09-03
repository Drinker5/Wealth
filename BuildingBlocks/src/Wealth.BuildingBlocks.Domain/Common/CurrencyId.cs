using System.Text.Json;
using System.Text.Json.Serialization;

namespace Wealth.BuildingBlocks.Domain.Common;

[JsonConverter(typeof(CurrencyIdConverter))]
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

        if (!TryParse(code, out var value))
            throw new ArgumentException($"Can't parse currency id {code}");

        Value = value;
    }

    public static bool TryParse(string value, out CurrencyCode result) => Enum.TryParse(value, true, out result);

    public static implicit operator byte(CurrencyId id) => (byte)id.Value;

    public static implicit operator CurrencyId(byte code) => new((CurrencyCode)code);

    public static implicit operator CurrencyCode(CurrencyId id) => id.Value;

    public static implicit operator CurrencyId(CurrencyCode code) => new(code);

    public static implicit operator CurrencyId(string code) => new(code);

    public override string ToString() => Value.ToString();

    internal sealed class CurrencyIdConverter : JsonConverter<CurrencyId>
    {
        public override CurrencyId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return new CurrencyId(reader.GetByte());
        }

        public override void Write(Utf8JsonWriter writer, CurrencyId currencyId, JsonSerializerOptions options)
        {
            writer.WriteNumberValue((byte)currencyId.Value);
        }
    }
}