using Microsoft.Extensions.Options;
using Tinkoff.InvestApi.V1;
using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.PortfolioManagement.Application.Options;
using Wealth.PortfolioManagement.Application.Providers;

namespace Wealth.PortfolioManagement.Infrastructure.Providers;

internal sealed class TBankOperationProducer(
    TBankOperationProvider operationProvider,
    IKafkaProducer<Operation> producer) : IOperationProducer
{
    private const int chunkSize = 100;

    public async Task ProduceOperations(DateTimeOffset from, DateTimeOffset to, CancellationToken token)
    {
        await foreach (var chunk in operationProvider
                           .GetOperations(from, to, token)
                           .Chunk(chunkSize)
                           .WithCancellation(token))
            await ProduceChunk(chunk, token);
    }

    private Task ProduceChunk(Operation[] operations, CancellationToken token)
    {
        return operations.Length == 0 ? Task.CompletedTask : producer.Produce(operations, token);
    }
}