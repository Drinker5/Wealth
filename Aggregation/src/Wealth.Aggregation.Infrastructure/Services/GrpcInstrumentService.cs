using System.Collections.ObjectModel;
using Wealth.Aggregation.Application.Services;
using Wealth.BuildingBlocks;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement;

namespace Wealth.Aggregation.Infrastructure.Services;

public class GrpcInstrumentService(InstrumentsService.InstrumentsServiceClient client) : IInstrumentService
{
    public async Task<IReadOnlyDictionary<StockId, StockInfo>> GetStocksInfo(
        IReadOnlyCollection<StockId> stockIds,
        CancellationToken token)
    {
        if (stockIds.Count == 0)
            return ReadOnlyDictionary<StockId, StockInfo>.Empty;

        var request = new GetStocksRequest
        {
            StockIds = { stockIds.Select(i => (StockIdProto)i).ToArray() }
        };

        var response = await client.GetStocksAsync(request, cancellationToken: token);

        return response.StockInfos
            .Select(i => i.FromProto())
            .ToDictionary(i => i.Id, i => i);
    }

    public async Task<StockInfo?> GetStockInfo(StockId stockId)
    {
        var response = await client.GetStockAsync(new GetStockRequest { StockId = stockId });
        return response.StockInfo.FromProto();
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

    public static StockInfo FromProto(this StockInfoProto stock)
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