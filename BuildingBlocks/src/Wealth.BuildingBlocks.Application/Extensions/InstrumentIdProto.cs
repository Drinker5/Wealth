using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.BuildingBlocks;

public partial class InstrumentIdProto
{
    public static implicit operator InstrumentId(InstrumentIdProto grpcValue)
    {
        return new InstrumentId(grpcValue.Value);
    }

    public static implicit operator InstrumentIdProto(InstrumentId value)
    {
        return new InstrumentIdProto
        {
            Value = value.Value
        };
    }

    public static implicit operator InstrumentIdProto(Guid guid)
    {
        return new InstrumentIdProto
        {
            Value = guid
        };
    }

    public static implicit operator InstrumentIdProto(string guidAsString)
    {
        return new InstrumentIdProto
        {
            Value = Guid.Parse(guidAsString)
        };
    }
}