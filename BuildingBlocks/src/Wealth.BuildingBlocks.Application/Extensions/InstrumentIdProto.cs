using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.BuildingBlocks;

public partial class InstrumentIdProto
{
    public static implicit operator InstrumentUId(InstrumentIdProto grpcValue)
    {
        return new InstrumentUId(grpcValue.Value);
    }

    public static implicit operator InstrumentIdProto(InstrumentUId value)
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