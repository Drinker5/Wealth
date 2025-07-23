namespace Wealth.BuildingBlocks;

public partial class GuidProto
{
    public GuidProto(string id)
    {
        Value = id;
    }
    
    public static implicit operator Guid(GuidProto grpcValue)
    {
        return new Guid(grpcValue.Value);
    }

    public static implicit operator GuidProto(Guid value)
    {
        return new GuidProto(value.ToString("D"));
    }
}