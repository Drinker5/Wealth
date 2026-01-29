using System.Text.Json;
using System.Text.Json.Serialization;

namespace Wealth.BuildingBlocks.Domain.Common;

[JsonConverter(typeof(InstrumentUIdConverter))]
public readonly record struct InstrumentUId(Guid Value) : IIdentity
{
    public static InstrumentUId New() => new(Guid.NewGuid());
    public static InstrumentUId From(string value) => new(Guid.Parse(value));

    public override string ToString()
    {
        return Value.ToString("D");
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
    
    public static implicit operator Guid(InstrumentUId uId)
    {
        return uId.Value;
    }
    
    public static implicit operator InstrumentUId(Guid value)
    {
        return new InstrumentUId(value);
    }
    
    public static implicit operator InstrumentUId(string value)
    {
        return new InstrumentUId(Guid.Parse(value));
    }
    
    private class InstrumentUIdConverter : JsonConverter<InstrumentUId>
    {
        public override InstrumentUId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return new InstrumentUId(reader.GetGuid());
        }

        public override void Write(Utf8JsonWriter writer, InstrumentUId value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}