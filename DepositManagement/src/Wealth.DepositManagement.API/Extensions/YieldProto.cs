using Wealth.DepositManagement.Domain.Deposits;

namespace Wealth.BuildingBlocks.DepositManagement;

public partial class YieldProto
{
    public YieldProto(decimal value)
    {
        PercentPerYear = value;
    }
    
    public static implicit operator Yield(YieldProto grpcValue)
    {
        return new Yield(grpcValue.PercentPerYear);
    }

    public static implicit operator YieldProto(Yield value)
    {
        return new YieldProto(value.PercentPerYear);
    }
}