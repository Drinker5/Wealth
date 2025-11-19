using System.Text.Json;
using System.Text.Json.Serialization;

namespace Wealth.BuildingBlocks.Domain.Common;

[JsonConverter(typeof(CurrencyIdJsonConverter))]
public readonly record struct CurrencyId(int Value) : IIdentity
{
    public override string ToString()
    {
        return Value.ToString();
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public static implicit operator int(CurrencyId id)
    {
        return id.Value;
    }

    public static implicit operator CurrencyId(int value)
    {
        return new CurrencyId(value);
    }

    private class CurrencyIdJsonConverter : JsonConverter<CurrencyId>
    {
        public override CurrencyId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return new CurrencyId(reader.GetInt32());
        }

        public override void Write(Utf8JsonWriter writer, CurrencyId value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value.Value);
        }
    }
}