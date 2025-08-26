using System.Text.Json;
using System.Text.Json.Serialization;

namespace Wealth.BuildingBlocks.Domain.Common;

[JsonConverter(typeof(StockIdJsonConverter))]
public readonly record struct StockId(int Id) : IIdentity
{
    public override string ToString()
    {
        return Id.ToString();
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
    
    public static implicit operator int(StockId value)
    {
        return value.Id;
    }
    
    public static implicit operator StockId(int value)
    {
        return new StockId(value);
    }
}

public class StockIdJsonConverter : JsonConverter<StockId>
{
    public override StockId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return new StockId(reader.GetInt32());
    }

    public override void Write(Utf8JsonWriter writer, StockId value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value.Id);
    }
}