using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.PortfolioManagement.Application.Services;

public interface IInstrumentService
{
    Task<StockInstrumentInfo> GetStockInfo(StockId instrumentId);
    Task<BondInstrumentInfo> GetBondInfo(BondId instrumentId);
}

public class StockInstrumentInfo
{
    public StockId Id { get; set; }
    public string Name { get; set; }
    public Money Price { get; set; }
    public Money DividendPerYear { get; set; }
    public int LotSize { get; set; }
}

public class BondInstrumentInfo
{
    public BondId Id { get; set; }
    public string Name { get; set; }
    public Money Price { get; set; }
}