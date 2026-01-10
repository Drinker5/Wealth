using System.Threading.RateLimiting;
using Polly;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement.Application.Providers;

namespace Wealth.InstrumentManagement.Infrastructure.Providers;

public sealed class InstrumentPricesProviderDecorator(IInstrumentPricesProvider instrumentPricesProvider) : IInstrumentPricesProvider
{
    private readonly ResiliencePipeline pipeline = new ResiliencePipelineBuilder()
        .AddRateLimiter(new SlidingWindowRateLimiter(
            new SlidingWindowRateLimiterOptions
            {
                PermitLimit = 10,
                SegmentsPerWindow = 4,
                Window = TimeSpan.FromMinutes(1)
            })).Build();

    public Task<IReadOnlyDictionary<InstrumentUId, decimal>> ProvidePrices(
        IReadOnlyCollection<InstrumentUId> instrumentUIds,
        CancellationToken token)
    {
        return pipeline.ExecuteAsync(
            async ct => await instrumentPricesProvider.ProvidePrices(instrumentUIds, ct),
            token).AsTask();
    }
}