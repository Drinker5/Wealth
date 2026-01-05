using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.BuildingBlocks;

public partial class InstrumentUIdProto
{
    public static implicit operator InstrumentUId(InstrumentUIdProto grpcValue)
    {
        return new InstrumentUId(grpcValue.Value);
    }

    public static implicit operator InstrumentUIdProto(InstrumentUId value)
    {
        return new InstrumentUIdProto
        {
            Value = value.Value
        };
    }

    public static implicit operator InstrumentUIdProto(Guid guid)
    {
        return new InstrumentUIdProto
        {
            Value = guid
        };
    }

    public static implicit operator InstrumentUIdProto(string guidAsString)
    {
        return new InstrumentUIdProto
        {
            Value = Guid.Parse(guidAsString)
        };
    }
}