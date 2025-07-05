using System.Data;
using Dapper;
using Dommel;
using Wealth.InstrumentManagement.Domain.Instruments;

namespace Wealth.InstrumentManagement.Infrastructure.Dapper;

public static class DapperMapping
{
    public static void Map()
    {
        DommelMapper.SetKeyPropertyResolver(new KeyResolver());
        DommelMapper.SetTableNameResolver(new CustomTableNameResolver());

        SqlMapper.AddTypeHandler(typeof(InstrumentId), new InstrumentIdHandler());
    }
}

public class CustomTableNameResolver : ITableNameResolver
{
    public string ResolveTableName(Type type)
    {
        if (type.IsAssignableTo(typeof(Instrument)))
            return "Instruments";

        return type.Name;
    }
}

public class InstrumentIdHandler : SqlMapper.TypeHandler<InstrumentId>
{
    public override void SetValue(IDbDataParameter parameter, InstrumentId? value)
    {
        parameter.Value = value?.Id;
    }

    public override InstrumentId Parse(object value)
    {
        return new InstrumentId((Guid)value);
    }
}

