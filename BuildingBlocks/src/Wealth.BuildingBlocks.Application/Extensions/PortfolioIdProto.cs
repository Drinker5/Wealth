using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.BuildingBlocks;

public partial class PortfolioIdProto
{
    public PortfolioIdProto(int id)
    {
        Id = id;
    }
    
    public static implicit operator PortfolioId(PortfolioIdProto grpcValue)
    {
        return new PortfolioId(grpcValue.Id);
    }

    public static implicit operator PortfolioIdProto(PortfolioId value)
    {
        return new PortfolioIdProto(value.Id);
    }
}