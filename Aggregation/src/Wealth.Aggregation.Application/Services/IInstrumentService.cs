using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.Aggregation.Application.Services;

public interface IInstrumentService
{
    Task<IReadOnlyDictionary<StockId, StockInfo>> GetStocksInfo(
        IReadOnlyCollection<StockId> stockIds,
        CancellationToken token);

    Task<BondInfo?> GetBondInfo(BondId bondId);
}

public abstract class InstrumentInfo
{
    public string Name { get; set; }
}

public class StockInfo : InstrumentInfo
{
    public StockId Id { get; set; }
    public Ticker Ticker { get; set; }
    public Money DividendPerYear { get; set; }
    public int LotSize { get; set; }
}

public class BondInfo : InstrumentInfo
{
    public BondId Id { get; set; }
}