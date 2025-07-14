using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement;
using Wealth.PortfolioManagement.Application.Services;
using InstrumentType = Wealth.InstrumentManagement.InstrumentType;

namespace Wealth.PortfolioManagement.Infrastructure.Services;

public class GrpcInstrumentService(InstrumentsService.InstrumentsServiceClient client) : IInstrumentService
{
    public async Task<InstrumentInfo> GetInstrumentInfo(InstrumentId instrumentId)
    {
        var response = await client.GetInstrumentAsync(new GetInstrumentRequest { Id = instrumentId.ToString() });
        return new InstrumentInfo
        {
            Id = instrumentId,
            Type = Convert(response.Type),
            Name = response.Name,
        };
    }

    private Application.Services.InstrumentType Convert(InstrumentType instrumentType)
    {
        return instrumentType switch
        {
            InstrumentType.ItStock => Application.Services.InstrumentType.Stock,
            InstrumentType.ItBond => Application.Services.InstrumentType.Bond,
            _ => throw new ArgumentOutOfRangeException(nameof(instrumentType), instrumentType, null)
        };
    }
}

