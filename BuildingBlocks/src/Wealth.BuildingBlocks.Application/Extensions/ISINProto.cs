using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.BuildingBlocks;

public partial class ISINProto
{
    public ISINProto(string value)
    {
        Value = value;
    }

    public static implicit operator ISIN(ISINProto grpcValue)
    {
        return new ISIN(grpcValue.Value);
    }

    public static implicit operator ISINProto(ISIN value)
    {
        return new ISINProto(value.Value);
    }
    
    public static implicit operator ISINProto(string value)
    {
        return new ISINProto(value);
    }
}