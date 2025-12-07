using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Wealth.Aggregation.Application.Services;
using Wealth.BuildingBlocks.Infrastructure;

namespace Wealth.Aggregation.Infrastructure.Services;

public class ServicesModule : IServiceModule
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IInstrumentService, GrpcInstrumentService>();
        services.AddSingleton<IStockAggregationService, StockAggregationService>();
        services.AddSingleton<IStrategyService, GrpcStrategyService>();
    }
}