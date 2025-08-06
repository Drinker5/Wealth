using Wealth.Aggregation.Application.Services;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement;

namespace Wealth.Aggregation.API.Services;

public class GrpcInstrumentService(InstrumentsService.InstrumentsServiceClient client) : IInstrumentService
{
    public Task<StockInfo> GetBondInfo(StockId stockId)
    {
        var response = await client.GetInstrumentAsync(new GetInstrumentRequest { Id = instrumentId });
        return response.FromProto();
    }

    public Task<BondInfo> GetBondInfo(BondId bondId)
    {
        var response = await client.GetInstrumentAsync(new GetInstrumentRequest { Id = instrumentId });
        return response.FromProto();
    }
}

internal static class ProtoConverters
{
    public static InstrumentInfo FromProto(this InstrumentProto instrument)
    {
        InstrumentInfo info = instrument.TypeCase switch
        {
            InstrumentProto.TypeOneofCase.StockInfo => new StockInstrumentInfo
            {
                DividendPerYear = instrument.StockInfo.DividendPerYear,
                LotSize = instrument.StockInfo.LotSize,
            },
            InstrumentProto.TypeOneofCase.BondInfo => new BondInstrumentInfo
            {
            },
            _ => throw new ArgumentOutOfRangeException()
        };
        
        info.Id = instrument.Id;
        info.Name = instrument.Name;
        info.Price = instrument.Price;
        return info;
    }
}