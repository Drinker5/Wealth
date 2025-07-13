using Grpc.Core;
using MediatR;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement.Application.Instruments.Commands;
using Wealth.InstrumentManagement.Application.Instruments.Queries;

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

    public override async Task<ChangePriceResponse> ChangePrice(ChangePriceRequest request, ServerCallContext context)
    {
        var id = new InstrumentId(Guid.Parse(request.Id));
        var instrument = await mediator.Send<InstrumentDTO?>(new GetInstrumentQuery(id));
        if (instrument == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Instrument not found"));

        await mediator.Send(new ChangePriceCommand
        {
            Id = Guid.Parse(request.Id),
            Price = new Money(instrument.Price.code, Convert.ToDecimal(request.Price)),
        });
        
        return new ChangePriceResponse();
    }

}