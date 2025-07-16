namespace Wealth.InstrumentManagement;

public partial class GuidProto
{
    public GuidProto(string id)
    {
        Value = id;
    }
    
    public static implicit operator Guid(Wealth.InstrumentManagement.GuidProto grpcValue)
    {
        return new Guid(grpcValue.Value);
    }

    public static implicit operator Wealth.InstrumentManagement.GuidProto(Guid value)
    {
        return new GuidProto(value.ToString("N"));
    }
}