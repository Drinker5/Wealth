using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement;
using Wealth.PortfolioManagement.Application.Services;

namespace Wealth.PortfolioManagement.API.Services;

public class GrpcInstrumentService(InstrumentsService.InstrumentsServiceClient client) : IInstrumentService
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