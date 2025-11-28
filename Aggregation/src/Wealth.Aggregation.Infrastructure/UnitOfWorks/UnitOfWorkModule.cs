using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Octonica.ClickHouseClient;
using SharpJuice.Clickhouse;
using Wealth.Aggregation.Application.Repository;
using Wealth.Aggregation.Infrastructure.Repositories;
using Wealth.BuildingBlocks.Infrastructure;

namespace Wealth.Aggregation.Infrastructure.UnitOfWorks;

public class UnitOfWorkModule : IServiceModule
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IClickHouseConnectionFactory>(_ =>
        {
            var builder = new ClickHouseConnectionStringBuilder
            {
                ConnectionString = configuration.GetConnectionString("ClickHouse")
            };
            return new ClickHouseConnectionFactory(builder.BuildSettings());
        });

        services.AddSingleton<ITableWriterBuilder, TableWriterBuilder>();
        services.AddSingleton<IStockTradeOperationRepository, StockTradeOperationRepository>();
        services.AddSingleton<ICurrencyTradeOperationRepository, CurrencyTradeOperationRepository>();
        services.AddSingleton<IStockMoneyOperationRepository, StockMoneyOperationRepository>();
        services.AddSingleton<IBondMoneyOperationRepository, BondMoneyOperationRepository>();
        services.AddSingleton<ICurrencyMoneyOperationRepository, CurrencyMoneyOperationRepository>();
        services.AddSingleton<IMoneyOperationRepository, MoneyOperationRepository>();

        services.AddSingleton<IStockAggregationRepository, StockAggregationRepository>();
    }
}