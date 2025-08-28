using System.Data;
using Dapper;
using Dommel;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement.Domain.Instruments;

namespace Wealth.InstrumentManagement.Infrastructure.Dapper;

public static class DapperMapping
{
    public static void Map()
    {
        DommelMapper.SetKeyPropertyResolver(new KeyResolver());
        DommelMapper.SetTableNameResolver(new CustomTableNameResolver());

        SqlMapper.AddTypeHandler(typeof(StockId), new StockIdHandler());
    }
}

public class CustomTableNameResolver : ITableNameResolver
{
    public string ResolveTableName(Type type)
    {
        if (type.IsAssignableTo(typeof(Stock)))
            return "Stocks";
        if (type.IsAssignableTo(typeof(Bond)))
            return "Bonds";
        return type.Name;
    }
}

public class StockIdHandler : SqlMapper.TypeHandler<StockId>
{
    public override void SetValue(IDbDataParameter parameter, StockId value)
    {
        parameter.Value = value.Value;
    }

    public override StockId Parse(object value)
    {
        return new StockId((int)value);
    }
}