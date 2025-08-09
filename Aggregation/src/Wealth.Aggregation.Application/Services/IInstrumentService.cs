using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.Aggregation.Application.Services;

public interface IInstrumentService
{
    Task<StockInfo?> GetStockInfo(StockId stockId);
    Task<BondInfo?> GetBondInfo(BondId bondId);
}

public abstract class InstrumentInfo
{
    public string Name { get; set; }
    public Money Price { get; set; }
}

public class StockInfo : InstrumentInfo
{
    public StockId Id { get; set; }
    public Money DividendPerYear { get; set; }
    public int LotSize { get; set; }
}

public class BondInfo : InstrumentInfo
{
    public BondId Id { get; set; }
}
