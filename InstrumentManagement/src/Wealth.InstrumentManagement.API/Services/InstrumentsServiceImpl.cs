using Grpc.Core;
using Wealth.BuildingBlocks.Application;
using Wealth.InstrumentManagement.Application.Instruments.Commands;
using Wealth.InstrumentManagement.Application.Instruments.Queries;

namespace Wealth.InstrumentManagement.API.Services;

public class InstrumentsServiceImpl(ICqrsInvoker mediator) : InstrumentsService.InstrumentsServiceBase
{
    public override async Task<CreateStockResponse> CreateStock(CreateStockRequest request, ServerCallContext context)
    {
        try
        {
            var stockId = await mediator.Command(new CreateStockCommand
            {
                Ticker = request.Ticker,
                Name = request.Name,
                Isin = request.Isin,
                Figi = request.Figi,
                LotSize = request.LotSize,
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
            Isin = request.Isin,
            Figi = request.Figi,
        });

        return new CreateBondResponse
        {
            BondId = bondId
        };
    }

    public override async Task<CreateCurrencyResponse> CreateCurrency(CreateCurrencyRequest request, ServerCallContext context)
    {
        var currencyId = await mediator.Command(new CreateCurrencyCommand
        {
            Name = request.Name,
            Figi = request.Figi,
        });

        return new CreateCurrencyResponse
        {
            CurrencyId = currencyId
        };
    }

    public override async Task<GetBondResponse> GetBond(GetBondRequest request, ServerCallContext context)
    {
        var instrument = request.VariantCase switch
        {
            GetBondRequest.VariantOneofCase.BondId => await mediator.Query(new GetBond(request.BondId)),
            GetBondRequest.VariantOneofCase.Isin => await mediator.Query(new GetBondByIsin(request.Isin)),
            GetBondRequest.VariantOneofCase.Figi => await mediator.Query(new GetBondByFigi(request.Figi)),
            _ => null
        };

        if (instrument == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Instrument not found"));

        var response = new GetBondResponse
        {
            BondId = instrument.Id,
            Name = instrument.Name,
            Price = instrument.Price,
            Isin = instrument.Isin,
            Figi = instrument.Figi,
        };

        return response;
    }

    public override async Task<GetCurrencyResponse> GetCurrency(GetCurrencyRequest request, ServerCallContext context)
    {
        var instrument = request.VariantCase switch
        {
            GetCurrencyRequest.VariantOneofCase.CurrencyId => await mediator.Query(new GetCurrency(request.CurrencyId)),
            GetCurrencyRequest.VariantOneofCase.Figi => await mediator.Query(new GetCurrencyByFigi(request.Figi)),
            _ => null
        };

        if (instrument == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Instrument not found"));

        var response = new GetCurrencyResponse
        {
            CurrencyId = instrument.Id,
            Name = instrument.Name,
            Price = instrument.Price,
            Figi = instrument.Figi,
        };

        return response;
    }

    public override async Task<GetStockResponse> GetStock(GetStockRequest request, ServerCallContext context)
    {
        var instrument = request.VariantCase switch
        {
            GetStockRequest.VariantOneofCase.StockId => await mediator.Query(new GetStock(request.StockId)),
            GetStockRequest.VariantOneofCase.Isin => await mediator.Query(new GetStockByIsin(request.Isin)),
            GetStockRequest.VariantOneofCase.Figi => await mediator.Query(new GetStockByFigi(request.Figi)),
            _ => null
        };

        if (instrument == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Instrument not found"));

        var response = new GetStockResponse
        {
            StockInfo = new StockInfoProto
            {
                StockId = instrument.Id,
                Name = instrument.Name,
                Price = instrument.Price,
                Isin = instrument.Isin,
                Figi = instrument.Figi,
                DividendPerYear = instrument.Dividend.ValuePerYear,
                LotSize = instrument.LotSize,
                Ticker = instrument.Ticker
            }
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

    public override async Task<ChangePriceResponse> ChangeCurrencyPrice(ChangeCurrencyPriceRequest request, ServerCallContext context)
    {
        var instrument = await mediator.Query(new GetCurrency(request.CurrencyId));
        if (instrument == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Instrument not found"));

        await mediator.Command(new ChangeCurrencyPriceCommand
        {
            CurrencyId = request.CurrencyId,
            Price = request.Price,
        });

        return new ChangePriceResponse();
    }

    public override Task<GetInstrumentsResponse> GetInstrumentsByIsin(GetInstrumentsByIsinRequest request, ServerCallContext context)
    {
        // TODO
        throw new RpcException(new Status(StatusCode.Unimplemented, "Unimplemented"));
    }
}