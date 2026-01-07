using Eventso.Subscription;
using Microsoft.Extensions.Options;
using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.PortfolioManagement.Application.Options;
using Wealth.PortfolioManagement.Application.Portfolios.Commands;
using Wealth.PortfolioManagement.Application.Providers;

namespace Wealth.PortfolioManagement.Infrastructure.Providers.Handling;

public sealed class OperationHandler(
    IPortfolioIdProvider portfolioIdProvider,
    OperationConverter converter,
    ICqrsInvoker mediator,
    IInstrumentIdProvider instrumentIdProvider,
    IOptions<TBankOperationProviderOptions> options) : IMessageHandler<IReadOnlyCollection<Tinkoff.InvestApi.V1.Operation>>
{
    public async Task Handle(IReadOnlyCollection<Tinkoff.InvestApi.V1.Operation> operations, CancellationToken token)
    {
        var portfolioId = await portfolioIdProvider.GetPortfolioIdByAccountId(options.Value.AccountId, token);
        var instrumentIdTypes = operations
            .Where(i => !string.IsNullOrWhiteSpace(i.InstrumentUid))
            .Select(i => Parse(i.InstrumentUid, i.InstrumentType))
            .ToHashSet();

        // 2f086529-3b6b-493d-b5d5-30e7051de52f to
        // "uid": "83cbf397-6551-47c8-b0f9-8bf7fbc4c1cc",
        var existedInstruments = await instrumentIdProvider.GetInstruments(instrumentIdTypes, token);

        foreach (var operation in operations)
            await Handle(operation, portfolioId, token);
    }

    private async Task Handle(Tinkoff.InvestApi.V1.Operation operation, PortfolioId portfolioId, CancellationToken token)
    {
        await foreach (var converted in converter.ConvertOperation(operation, portfolioId, token))
            await mediator.Command(new AddOperation(converted), token);
    }

    private static InstrumentUIdType Parse(string Id, string Type) => new(Guid.Parse(Id), Parse(Type));

    private static InstrumentType Parse(string type)
    {
        if (string.IsNullOrWhiteSpace(type))
            throw new ArgumentException("Instrument type cannot be null or empty", nameof(type));

        return type.ToLowerInvariant() switch
        {
            "share" => InstrumentType.Stock,
            "bond" => InstrumentType.Bond,
            "currency" => InstrumentType.CurrencyAsset,
            _ => throw new ArgumentException($"Invalid instrument type: {type}. Valid values are: Share, Bond, Currency", nameof(type))
        };
    }
}