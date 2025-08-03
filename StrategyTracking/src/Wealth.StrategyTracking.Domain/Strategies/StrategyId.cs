using System.Text.Json;
using System.Text.Json.Serialization;
using Wealth.BuildingBlocks.Domain;

namespace Wealth.StrategyTracking.Domain.Strategies;

[JsonConverter(typeof(StrategyIdJsonConverter))]
public readonly record struct StrategyId(int Id) : IIdentity
{
    public static StrategyId New() => new StrategyId(0);

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public override string ToString()
    {
        return Id.ToString();
    }
    
    public static implicit operator int(StrategyId id)
    {
        return id.Id;
    }
    
    public static implicit operator StrategyId(int id)
    {
        return new StrategyId(id);
    }
}

public class StrategyIdJsonConverter : JsonConverter<StrategyId>
{
    public override StrategyId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return new StrategyId(reader.GetInt32());
    }

    public override void Write(Utf8JsonWriter writer, StrategyId value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value.Id);
    }
}