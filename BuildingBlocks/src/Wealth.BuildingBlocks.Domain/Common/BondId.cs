using System.Text.Json;
using System.Text.Json.Serialization;

namespace Wealth.BuildingBlocks.Domain.Common;

[JsonConverter(typeof(BondIdJsonConverter))]
public readonly record struct BondId(int Id) : IIdentity
{
    public override string ToString()
    {
        return Id.ToString();
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
    
    public static implicit operator int(BondId value)
    {
        return value.Id;
    }
    
    public static implicit operator BondId(int value)
    {
        return new BondId(value);
    }
}

public class BondIdJsonConverter : JsonConverter<BondId>
{
    public override BondId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return new BondId(reader.GetInt32());
    }

    public override void Write(Utf8JsonWriter writer, BondId value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value.Id);
    }
}