using Grpc.Core;
using Wealth.BuildingBlocks.Application;
using Wealth.InstrumentManagement.Application.Instruments.Commands;
using Wealth.InstrumentManagement.Application.Instruments.Queries;

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
                StockId = stockId
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
            BondId = bondId
        };
    }

    public override async Task<GetBondResponse> GetBond(GetBondRequest request, ServerCallContext context)
    {
        var id = request.BondId;
        var instrument = await mediator.Query(new GetBond(id));
        if (instrument == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Instrument not found"));

        var response = new GetBondResponse
        {
            BondId = instrument.Id,
            Name = instrument.Name,
            Price = instrument.Price,
            Isin = instrument.ISIN,
        };

        return response;
    }

    public override async Task<GetStockResponse> GetStock(GetStockRequest request, ServerCallContext context)
    {
        var instrument = await mediator.Query(new GetStock(request.StockId));
        if (instrument == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Instrument not found"));

        var response = new GetStockResponse
        {
            StockId = instrument.Id,
            Name = instrument.Name,
            Price = instrument.Price,
            Isin = instrument.ISIN,
            DividendPerYear = instrument.Dividend.ValuePerYear,
            LotSize = instrument.LotSize,
        };

        return response;
    }

    public override async Task<ChangePriceResponse> ChangeStockPrice(ChangeStockPriceRequest request, ServerCallContext context)
    {
        var instrument = await mediator.Query(new GetStock(request.StockId));
        if (instrument == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Instrument not found"));

        await mediator.Command(new ChangeStockPriceCommand
        {
            StockId = request.StockId,
            Price = request.Price,
        });

        return new ChangePriceResponse();
    }
    
    public override async Task<ChangePriceResponse> ChangeBondPrice(ChangeBondPriceRequest request, ServerCallContext context)
    {
        var instrument = await mediator.Query(new GetBond(request.BondId));
        if (instrument == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Instrument not found"));

        await mediator.Command(new ChangeBondPriceCommand
        {
            BondId = request.BondId,
            Price = request.Price,
        });

        return new ChangePriceResponse();
    }
}