using Dommel;

namespace Wealth.InstrumentManagement.Infrastructure.Dapper;

public class KeyResolver : IKeyPropertyResolver
{
    public ColumnPropertyInfo[] ResolveKeyProperties(Type type)
    {
        return [new ColumnPropertyInfo(type.GetProperties().Single(p => p.Name == $"Id"), isKey: true)];
    }
}