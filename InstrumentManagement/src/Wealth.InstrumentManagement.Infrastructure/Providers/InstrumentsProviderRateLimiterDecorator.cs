using System.Threading.RateLimiting;
using Polly;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement.Application.Instruments.Commands;
using Wealth.InstrumentManagement.Application.Providers;

namespace Wealth.InstrumentManagement.Infrastructure.Providers;

public sealed class InstrumentsProviderRateLimiterDecorator(IInstrumentsProvider instrumentsProvider) : IInstrumentsProvider
{
    private readonly ResiliencePipeline pipeline = new ResiliencePipelineBuilder()
        .AddRateLimiter(new SlidingWindowRateLimiter(
            new SlidingWindowRateLimiterOptions
            {
                PermitLimit = 60,
                SegmentsPerWindow = 4,
                Window = TimeSpan.FromMinutes(1)
            })).Build();

    public ValueTask<CreateStockCommand> StockProvide(InstrumentId instrumentId, CancellationToken token)
    {
        return pipeline.ExecuteAsync(
            ct => instrumentsProvider.StockProvide(instrumentId, ct),
            token);
    }

    public ValueTask<CreateBondCommand> BondProvide(InstrumentId instrumentId, CancellationToken token)
    {
        return pipeline.ExecuteAsync(
            ct => instrumentsProvider.BondProvide(instrumentId, ct),
            token);
    }

    public ValueTask<CreateCurrencyCommand> CurrencyProvide(InstrumentId instrumentId, CancellationToken token)
    {
        return pipeline.ExecuteAsync(
            ct => instrumentsProvider.CurrencyProvide(instrumentId, ct),
            token);
    }
}