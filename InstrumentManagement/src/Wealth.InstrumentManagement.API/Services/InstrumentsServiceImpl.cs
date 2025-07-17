using Grpc.Core;
using MediatR;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.BuildingBlocks.InstrumentManagement;
using Wealth.InstrumentManagement.Application.Instruments.Commands;
using Wealth.InstrumentManagement.Application.Instruments.Queries;
using Wealth.InstrumentManagement.Domain.Instruments;

namespace Wealth.InstrumentManagement.API.Services;

public class InstrumentsServiceImpl : InstrumentsService.InstrumentsServiceBase
{
    private readonly ILogger<InstrumentsServiceImpl> _logger;
    private readonly IMediator mediator;

    public InstrumentsServiceImpl(ILogger<InstrumentsServiceImpl> logger, IMediator mediator)
    {
        _logger = logger;
        this.mediator = mediator;
    }

    public override async Task<GetInstrumentResponse> GetInstrument(GetInstrumentRequest request, ServerCallContext context)
    {
        var id = request.Id;
        var instrument = await mediator.Send<Instrument?>(new GetInstrumentQuery(id));
        if (instrument == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Instrument not found"));

        var response = new GetInstrumentResponse
        {
            Id = request.Id,
            Name = instrument.Name,
            Price = instrument.Price,
        };

        switch (instrument)
        {
            case StockInstrument stock:
                response.StockInfo = new StockInstrumentProto
                {
                    DividendPerYear = stock.Dividend.ValuePerYear,
                    LotSize = stock.LotSize,
                };
                break;
            case BondInstrument bond:
                response.BondInfo = new BondInstrumentProto();
                break;
            default:
                throw new RpcException(new Status(StatusCode.NotFound, "Instrument type is not defined"));
        }

        return response;
    }

    public override async Task<ChangePriceResponse> ChangePrice(ChangePriceRequest request, ServerCallContext context)
    {
        InstrumentId id = request.Id;
        var instrument = await mediator.Send<Instrument?>(new GetInstrumentQuery(id));
        if (instrument == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Instrument not found"));

        await mediator.Send(new ChangePriceCommand
        {
            Id = request.Id,
            Price = instrument.Price,
        });

        return new ChangePriceResponse();
    }
}