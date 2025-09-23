using System.Collections.Frozen;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Tinkoff.InvestApi;
using Tinkoff.InvestApi.V1;
using Wealth.BuildingBlocks.Infrastructure.Kafka;
using Wealth.PortfolioManagement.Application.Providers;
using Timestamp = Google.Protobuf.WellKnownTypes.Timestamp;

namespace Wealth.PortfolioManagement.Infrastructure.Providers;

internal sealed class TBankOperationProducer(
    IPortfolioIdProvider portfolioIdProvider,
    IKafkaProducer producer,
    IOptions<TBankOperationProviderOptions> options) : IOperationProducer
{
    private readonly InvestApiClient client = InvestApiClientFactory.Create(options.Value.Token);

    public async Task ProduceOperations(DateTimeOffset from, CancellationToken token)
    {
        var operations = await client.Operations.GetOperationsAsync(new OperationsRequest
        {
            AccountId = options.Value.AccountId,
            From = Timestamp.FromDateTimeOffset(from.ToUniversalTime()),
            To = Timestamp.FromDateTime(DateTime.UtcNow)
        }, cancellationToken: token);

        if (operations.Operations.Count <= 0)
            return;

        var portfolioId = await portfolioIdProvider.GetPortfolioIdByAccountId(options.Value.AccountId, token);
        var enumerable = operations.Operations
            .Where(i => i.State == OperationState.Executed)
            .Select(i => new Message<string, Tinkoff.InvestApi.V1.Operation>
            {
                Key = portfolioId.ToString(),
                Value = i
            });

        await producer.ProduceAsync("operations", enumerable, token);
    }
}