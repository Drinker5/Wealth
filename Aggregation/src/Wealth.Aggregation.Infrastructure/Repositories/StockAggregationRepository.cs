using Microsoft.EntityFrameworkCore;
using Wealth.Aggregation.Domain;
using Wealth.Aggregation.Infrastructure.UnitOfWorks;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.Aggregation.Infrastructure.Repositories;

public class StockAggregationRepository(WealthDbContext context) : IStockAggregationRepository
{
    public async Task<StockAggregation?> GetStock(InstrumentId id)
    {
        return await context.StockAggregations.SingleOrDefaultAsync(i => i.Id == id);
    }

    public async Task<IEnumerable<StockAggregation>> GetAggregation()
    {
        return await context.StockAggregations.ToListAsync();
    }

    public async Task Create(InstrumentId id, string name, Money price, Money dividendPerYear, int lotSize)
    {
        var stockAggregation = new StockAggregation(id, name, price, dividendPerYear, lotSize);
        await context.StockAggregations.AddAsync(stockAggregation);
    }

    public async Task ChangeName(InstrumentId id, string name)
    {
        var stockAggregation = await GetStock(id);
        if (stockAggregation != null)
        {
            stockAggregation.ChangeName(name);
            await context.SaveChangesAsync();
        }
    }

    public async Task ChangePrice(InstrumentId id, Money price)
    {
        var stockAggregation = await GetStock(id);
        if (stockAggregation != null)
        {
            stockAggregation.ChangePrice(price);
            await context.SaveChangesAsync();
        }
    }

    public async Task ChangeDividendPerYear(InstrumentId id, Money dividendPerYear)
    {
        var stockAggregation = await GetStock(id);
        if (stockAggregation != null)
        {
            stockAggregation.ChangeDividendPerYear(dividendPerYear);
            await context.SaveChangesAsync();
        }
    }

    public async Task ChangeLotSize(InstrumentId id, int lotSize)
    {
        var stockAggregation = await GetStock(id);
        if (stockAggregation != null)
        {
            stockAggregation.ChangeLotSize(lotSize);
            await context.SaveChangesAsync();
        }
    }

    public async Task Buy(InstrumentId id, int quantity, Money investment)
    {
        var stockAggregation = await GetStock(id);
        if (stockAggregation != null)
        {
            stockAggregation.Buy(quantity, investment);
            await context.SaveChangesAsync();
        }
    }

    public async Task Sell(InstrumentId id, int quantity, Money profit)
    {
        var stockAggregation = await GetStock(id);
        if (stockAggregation != null)
        {
            stockAggregation.Sell(quantity, profit);
            await context.SaveChangesAsync();
        }
    }

    public async Task AddDividend(InstrumentId id, Money dividend)
    {
        var stockAggregation = await GetStock(id);
        if (stockAggregation != null)
        {
            stockAggregation.AddDividend(dividend);
            await context.SaveChangesAsync();
        }
    }
}