using System.Text.Json;
using System.Text.Json.Serialization;

namespace Wealth.BuildingBlocks.Domain.Common;

[JsonConverter(typeof(StockIdJsonConverter))]
public readonly record struct StockId(int Value) : IIdentity
{
    public override string ToString()
    {
        return Value.ToString();
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public static implicit operator int(StockId id)
    {
        return id.Value;
    }

    public static implicit operator StockId(int value)
    {
        return new StockId(value);
    }

    private class StockIdJsonConverter : JsonConverter<StockId>
    {
        public override StockId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return new StockId(reader.GetInt32());
        }

        public override void Write(Utf8JsonWriter writer, StockId value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value.Value);
        }
    }
}