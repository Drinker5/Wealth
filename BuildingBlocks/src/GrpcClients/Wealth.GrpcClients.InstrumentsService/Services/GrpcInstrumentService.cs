using Wealth.BuildingBlocks.Application.Services;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.BuildingBlocks.InstrumentManagement;

namespace Wealth.GrpcClients.InstrumentsService.Services;

public class GrpcInstrumentService(BuildingBlocks.InstrumentManagement.InstrumentsService.InstrumentsServiceClient client) : IInstrumentService
{
    public async Task<InstrumentInfo> GetInstrumentInfo(InstrumentId instrumentId)
    {
        var response = await client.GetInstrumentAsync(new GetInstrumentRequest { Id = instrumentId });
        return response.FromProto();
    }
}

internal static class ProtoConverters
{
    public static InstrumentInfo FromProto(this GetInstrumentResponse response)
    {
        InstrumentInfo info = response.InstrumentCase switch
        {
            GetInstrumentResponse.InstrumentOneofCase.StockInfo => new StockInstrumentInfo
            {
                DividendPerYear = response.StockInfo.DividendPerYear,
                LotSize = response.StockInfo.LotSize,
            },
            GetInstrumentResponse.InstrumentOneofCase.BondInfo => new BondInstrumentInfo
            {
            },
            _ => throw new ArgumentOutOfRangeException()
        };
        
        info.Id = response.Id;
        info.Name = response.Name;
        info.Price = response.Price;
        return info;
    }
}