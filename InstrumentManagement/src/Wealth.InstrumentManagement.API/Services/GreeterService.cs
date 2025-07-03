using Grpc.Core;
using MediatR;
using Wealth.InstrumentManagement.API;
using Wealth.InstrumentManagement.Application.Instruments.Commands;
using Wealth.InstrumentManagement.Application.Services;
using Wealth.InstrumentManagement.Domain.Instruments;

namespace Wealth.InstrumentManagement.API.Services;

public class GreeterService : Greeter.GreeterBase
{
    private readonly ILogger<GreeterService> _logger;
    private readonly IMediator mediator;

    public GreeterService(ILogger<GreeterService> logger, IMediator mediator)
    {
        _logger = logger;
        this.mediator = mediator;
    }

    public override async Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
        var id = await mediator.Send<InstrumentId>(new CreateStockCommand
        {
            ISIN = ISIN.Empty,
            Name = request.Name
        });

        return new HelloReply
        {
            Message = $"{id}",
        };
    }
}