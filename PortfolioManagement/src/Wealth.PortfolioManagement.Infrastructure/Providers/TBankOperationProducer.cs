using Microsoft.Extensions.Options;
using Tinkoff.InvestApi.V1;
using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.PortfolioManagement.Application.Providers;

namespace Wealth.PortfolioManagement.Infrastructure.Providers;

internal sealed class TBankOperationProducer(
    IPortfolioIdProvider portfolioIdProvider,
    TBankOperationProvider operationProvider,
    IKafkaProducer producer,
    IOptions<TBankOperationProviderOptions> options) : IOperationProducer
{
    private const int chunkSize = 100;

    public async Task ProduceOperations(DateTimeOffset from, DateTimeOffset to, CancellationToken token)
    {
        var portfolioId = await portfolioIdProvider.GetPortfolioIdByAccountId(options.Value.AccountId, token);
        await foreach (var chunk in operationProvider
                           .GetOperations(from, to, token)
                           .Chunk(chunkSize)
                           .WithCancellation(token))
            await ProduceChunk(portfolioId, chunk, token);
    }

    private async Task ProduceChunk(PortfolioId portfolioId, Operation[] operations, CancellationToken token)
    {
        if (operations.Length == 0)
            return;

        var messages = operations
            .Select(i => new BusMessage<string, Operation>
            {
                Key = portfolioId.ToString(),
                Value = i
            });

        await producer.ProduceAsync("wealth-operations", messages, token);
    }
}