using System.Collections.Frozen;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Options;
using Tinkoff.InvestApi;
using Tinkoff.InvestApi.V1;
using Wealth.PortfolioManagement.Application.Providers;
using Wealth.PortfolioManagement.Infrastructure.Providers.Handling;
using Operation = Wealth.PortfolioManagement.Domain.Operations.Operation;

namespace Wealth.PortfolioManagement.Infrastructure.Providers;

internal sealed class TBankOperationProvider(
    IPortfolioIdProvider portfolioIdProvider,
    OperationConverter converter,
    IOptions<TBankOperationProviderOptions> options) : IOperationProvider
{
    private readonly InvestApiClient client = InvestApiClientFactory.Create(options.Value.Token);

    public async IAsyncEnumerable<Operation> GetOperations(DateTimeOffset from)
    {
        var operations = await client.Operations.GetOperationsAsync(new OperationsRequest
        {
            AccountId = options.Value.AccountId,
            From = Timestamp.FromDateTimeOffset(from.ToUniversalTime()),
            To = Timestamp.FromDateTime(DateTime.UtcNow)
        });

        if (operations.Operations.Count <= 0)
            yield break;
        
        var portfolioId = await portfolioIdProvider.GetPortfolioIdByAccountId(options.Value.AccountId);
        foreach (var operation in operations.Operations.Where(i => i.State == OperationState.Executed))
        {
            await foreach (var converted in converter.ConvertOperation(operation, portfolioId))
                yield return converted;
        }
    }
}