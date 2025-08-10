using Wealth.Aggregation.Application.Services;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement;

namespace Wealth.Aggregation.API.Services;

public class GrpcInstrumentService(InstrumentsService.InstrumentsServiceClient client) : IInstrumentService
{
    public async Task<StockInfo?> GetStockInfo(StockId stockId)
    {
        var response = await client.GetStockAsync(new GetStockRequest { StockId = stockId });
        return response.FromProto();
    }

    public async Task<BondInfo?> GetBondInfo(BondId bondId)
    {
        var response = await client.GetBondAsync(new GetBondRequest { BondId = bondId });
        return response.FromProto();
    }
}

internal static class ProtoConverters
{
    public static BondInfo FromProto(this GetBondResponse bond)
    {
        BondInfo info = new()
        {
            Id = bond.BondId,
            Name = bond.Name,
            Price = bond.Price
        };

        return info;
    }

    public static StockInfo FromProto(this GetStockResponse stock)
    {
        StockInfo info = new()
        {
            DividendPerYear = stock.DividendPerYear,
            LotSize = stock.LotSize,
            Id = stock.StockId,
            Name = stock.Name,
            Price = stock.Price
        };

        return info;
    }
}