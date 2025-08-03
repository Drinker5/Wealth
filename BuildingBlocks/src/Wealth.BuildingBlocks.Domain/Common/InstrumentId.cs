using System.Text.Json;
using System.Text.Json.Serialization;

namespace Wealth.BuildingBlocks.Domain.Common;

[JsonConverter(typeof(InstrumentIdJsonConverter))]
public readonly record struct InstrumentId(Guid Id) : IIdentity
{
    public static InstrumentId New() => Guid.NewGuid();

    public static implicit operator Guid(InstrumentId id)
    {
        return id.Id;
    }
    
    public static implicit operator InstrumentId(Guid id)
    {
        return new InstrumentId(id);
    }

    public override string ToString()
    {
        return Id.ToString("N");
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}

public class InstrumentIdJsonConverter : JsonConverter<InstrumentId>
{
    public override InstrumentId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return new InstrumentId(reader.GetGuid());
    }

    public override void Write(Utf8JsonWriter writer, InstrumentId value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.Id.ToString("D"));
    }
}