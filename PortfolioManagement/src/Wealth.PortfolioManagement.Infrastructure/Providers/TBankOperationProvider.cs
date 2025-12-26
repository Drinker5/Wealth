using System.Collections;
using System.Runtime.CompilerServices;
using System.Threading.RateLimiting;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Options;
using Polly;
using Tinkoff.InvestApi;
using Tinkoff.InvestApi.V1;

namespace Wealth.PortfolioManagement.Infrastructure.Providers;

internal sealed class TBankOperationProvider(IOptions<TBankOperationProviderOptions> options)
{
    private readonly InvestApiClient client = InvestApiClientFactory.Create(options.Value.Token);
    private const int dateRangeSize = 30;

    private readonly ResiliencePipeline pipeline = new ResiliencePipelineBuilder()
        .AddRateLimiter(new SlidingWindowRateLimiter(
            new SlidingWindowRateLimiterOptions
            {
                PermitLimit = 30,
                SegmentsPerWindow = 4,
                Window = TimeSpan.FromMinutes(1)
            })).Build();

    public async IAsyncEnumerable<Operation> GetOperations(
        DateTimeOffset from,
        DateTimeOffset to,
        [EnumeratorCancellation] CancellationToken token)
    {
        foreach (var (chunkFrom, chunkTo) in SplitDateRange(from, to, dateRangeSize))
        {
            var operations = await pipeline.ExecuteAsync(
                async ct => await client.Operations.GetOperationsAsync(new OperationsRequest
                {
                    AccountId = options.Value.AccountId,
                    From = Timestamp.FromDateTimeOffset(chunkFrom.ToUniversalTime()),
                    To = Timestamp.FromDateTimeOffset(chunkTo.ToUniversalTime())
                }, cancellationToken: ct),
                token);

            if (operations.Operations.Count <= 0)
                yield break;

            foreach (var operation in operations.Operations.Where(i => i.State == OperationState.Executed))
                yield return operation;
        }
    }

    private static IEnumerable<(DateTimeOffset From, DateTimeOffset To)> SplitDateRange(
        DateTimeOffset from,
        DateTimeOffset to,
        int chunkDays)
    {
        DateTimeOffset chunkEnd;
        while ((chunkEnd = from.AddDays(chunkDays)) < to)
        {
            yield return (from, chunkEnd);
            from = chunkEnd.AddTicks(1);
        }

        yield return (from, to);
    }
}