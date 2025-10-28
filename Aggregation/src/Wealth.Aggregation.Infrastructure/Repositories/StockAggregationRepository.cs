using Microsoft.EntityFrameworkCore;
using SharpJuice.Clickhouse;
using Wealth.Aggregation.Domain;
using Wealth.Aggregation.Infrastructure.UnitOfWorks;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.Aggregation.Infrastructure.Repositories;

public class StockAggregationRepository(IClickHouseConnectionFactory connectionFactory) : IStockAggregationRepository
{
    public async Task<StockAggregation?> GetStock(StockId id)
    {
        return await context.StockAggregations.SingleOrDefaultAsync(i => i.Id == id);
    }

    public async Task<IEnumerable<StockAggregation>> GetAggregation()
    {
        return await context.StockAggregations.ToListAsync();
    }

    public async Task<StockAggregation> Create(StockId id, string name, Money stockPrice, Money dividendPerYear, int lotSize)
    {
        var stockAggregation = new StockAggregation(id, name, stockPrice, dividendPerYear, lotSize);
        await context.StockAggregations.AddAsync(stockAggregation);
        return stockAggregation;
    }

    public async Task ChangeName(StockId id, string name)
    {
        var stockAggregation = await GetStock(id);
        if (stockAggregation != null)
        {
            stockAggregation.ChangeName(name);
            await context.SaveChangesAsync();
        }
    }

    public async Task ChangePrice(StockId id, Money price)
    {
        var stockAggregation = await GetStock(id);
        if (stockAggregation != null)
        {
            stockAggregation.ChangePrice(price);
            await context.SaveChangesAsync();
        }
    }

    public async Task ChangeDividendPerYear(StockId id, Money dividendPerYear)
    {
        var stockAggregation = await GetStock(id);
        if (stockAggregation != null)
        {
            stockAggregation.ChangeDividendPerYear(dividendPerYear);
            await context.SaveChangesAsync();
        }
    }

    public async Task ChangeLotSize(StockId id, int lotSize)
    {
        var stockAggregation = await GetStock(id);
        if (stockAggregation != null)
        {
            stockAggregation.ChangeLotSize(lotSize);
            await context.SaveChangesAsync();
        }
    }

    public async Task Buy(PortfolioId portfolioId, StockId stockId, int quantity, Money investment, CancellationToken cancellationToken)
    {
        var stockAggregation = await GetStock(stockId);
        if (stockAggregation != null)
        {
            stockAggregation.Buy(quantity, investment);
            await context.SaveChangesAsync();
        }
    }

    public async Task Sell(PortfolioId portfolioId, StockId stockId, int quantity, Money profit, CancellationToken cancellationToken)
    {
        var stockAggregation = await GetStock(stockId);
        if (stockAggregation != null)
        {
            stockAggregation.Sell(quantity, profit);
            await context.SaveChangesAsync();
        }
    }

    public async Task AddDividend(StockId id, Money dividend)
    {
        var stockAggregation = await GetStock(id);
        if (stockAggregation != null)
        {
            stockAggregation.AddDividend(dividend);
            await context.SaveChangesAsync();
        }
    }
}