using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement;
using Wealth.PortfolioManagement.Application.Services;

namespace Wealth.PortfolioManagement.API.Services;

public class GrpcInstrumentService(InstrumentsService.InstrumentsServiceClient client) : IInstrumentService
{
    public async Task<StockInstrumentInfo> GetStockInfo(StockId instrumentId)
    {
        var response = await client.GetStockAsync(new GetStockRequest { StockId = instrumentId });
        return new StockInstrumentInfo
        {
            DividendPerYear = response.DividendPerYear,
            Id = response.StockId,
            LotSize = response.LotSize,
            Name = response.Name,
            Price = response.Price,
        };
    }

    public async Task<BondInstrumentInfo> GetBondInfo(BondId instrumentId)
    {
        var response = await client.GetBondAsync(new GetBondRequest { BondId = instrumentId });
        return new BondInstrumentInfo
        {
            Id = response.BondId,
            Name = response.Name,
            Price = response.Price,
        };
    }
}