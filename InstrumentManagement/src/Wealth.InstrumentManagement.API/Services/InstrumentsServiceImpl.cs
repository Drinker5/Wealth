using Grpc.Core;
using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement.Application.Instruments.Commands;
using Wealth.InstrumentManagement.Application.Instruments.Queries;
using Wealth.InstrumentManagement.Domain.Instruments;

namespace Wealth.InstrumentManagement.API.Services;

public class InstrumentsServiceImpl : InstrumentsService.InstrumentsServiceBase
{
    private readonly ILogger<InstrumentsServiceImpl> _logger;
    private readonly ICqrsInvoker mediator;

    public InstrumentsServiceImpl(ILogger<InstrumentsServiceImpl> logger, ICqrsInvoker mediator)
    {
        _logger = logger;
        this.mediator = mediator;
    }

    public override async Task<CreateStockResponse> CreateStock(CreateStockRequest request, ServerCallContext context)
    {
        try
        {
            var stockId = await mediator.Command(new CreateStockCommand
            {
                Name = request.Name,
                ISIN = request.Isin,
            });

            return new CreateStockResponse
            {
                Id = stockId
            };
        }
        catch (Exception ex)
        {
            throw new RpcException(new Status(StatusCode.Internal, ex.Message));
        }
    }

    public override async Task<CreateBondResponse> CreateBond(CreateBondRequest request, ServerCallContext context)
    {
        var bondId = await mediator.Command(new CreateBondCommand
        {
            Name = request.Name,
            ISIN = request.Isin,
        });

        return new CreateBondResponse
        {
            Id = bondId
        };
    }

    public override async Task<GetInstrumentResponse> GetInstrument(GetInstrumentRequest request, ServerCallContext context)
    {
        var id = request.Id;
        var instrument = await mediator.Query(new GetInstrumentQuery(id));
        if (instrument == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Instrument not found"));

        var response = new GetInstrumentResponse
        {
            Id = request.Id,
            Name = instrument.Name,
            Price = instrument.Price,
            Isin = instrument.ISIN,
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
        var instrument = await mediator.Query(new GetInstrumentQuery(id));
        if (instrument == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Instrument not found"));

        await mediator.Command(new ChangePriceCommand
        {
            Id = request.Id,
            Price = request.Price,
        });

        return new ChangePriceResponse();
    }
}