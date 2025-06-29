using Grpc.Core;
using Wealth.InstrumentManagement.API;
using Wealth.InstrumentManagement.Application.Services;

namespace Wealth.InstrumentManagement.API.Services;

public class GreeterService : Greeter.GreeterBase
{
    private readonly ILogger<GreeterService> _logger;
    private readonly ICurrencyService currencyService;

    public GreeterService(ILogger<GreeterService> logger, ICurrencyService currencyService)
    {
        _logger = logger;
        this.currencyService = currencyService;
    }

    public override async Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
        var isCurrencyExists = await currencyService.IsCurrencyExists(request.Name);
        return new HelloReply
        {
            Message = $"{isCurrencyExists}",
        };
    }
}