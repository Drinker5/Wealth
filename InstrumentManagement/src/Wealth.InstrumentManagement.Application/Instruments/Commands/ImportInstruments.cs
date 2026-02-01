using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement.Application.Instruments.Models;
using Wealth.InstrumentManagement.Application.Providers;
using Wealth.InstrumentManagement.Application.Repositories;

namespace Wealth.InstrumentManagement.Application.Instruments.Commands;

public readonly record struct ImportInstruments(
    IReadOnlyCollection<InstrumentUId>? StockInstrumentIds,
    IReadOnlyCollection<InstrumentUId>? BondInstrumentIds,
    IReadOnlyCollection<InstrumentUId>? CurrencyInstrumentIds) : ICommand<IReadOnlyCollection<Instrument>>;

public sealed class UpdateInstrumentsHandler(
    IInstrumentsProvider instrumentsProvider,
    IStocksRepository stocksRepository,
    IBondsRepository bondsRepository,
    ICurrenciesRepository currenciesRepository)
    : ICommandHandler<ImportInstruments, IReadOnlyCollection<Instrument>>
{
    public async Task<IReadOnlyCollection<Instrument>> Handle(ImportInstruments request, CancellationToken token)
    {
        var stockInstrumentIds = request.StockInstrumentIds?.ToHashSet() ?? [];
        var bondInstrumentIds = request.BondInstrumentIds?.ToHashSet() ?? [];
        var currencyInstrumentIds = request.CurrencyInstrumentIds?.ToHashSet() ?? [];

        var result = new List<Instrument>(stockInstrumentIds.Count + bondInstrumentIds.Count + currencyInstrumentIds.Count);
        if (stockInstrumentIds.Count > 0)
            result.AddRange(await ImportStocks(stockInstrumentIds, token));
        if (bondInstrumentIds.Count > 0)
            result.AddRange(await ImportBonds(bondInstrumentIds, token));
        if (currencyInstrumentIds.Count > 0)
            result.AddRange(await ImportCurrencies(currencyInstrumentIds, token));

        return result;
    }

    private async Task<IEnumerable<Instrument>> ImportStocks(IReadOnlyCollection<InstrumentUId> stockInstrumentIds, CancellationToken token)
    {
        var result = new List<Instrument>();
        foreach (var stockInstrumentId in stockInstrumentIds)
        {
            var command = await instrumentsProvider.StockProvide(stockInstrumentId, token);
            if (command == null)
                continue;

            var stockId = await stocksRepository.UpsertStock(command.Value, token);
            result.Add(new Instrument
            {
                InstrumentUId = stockInstrumentId,
                Id = stockId,
                Type = InstrumentType.Stock
            });
        }

        return result;
    }

    private async Task<IEnumerable<Instrument>> ImportBonds(IReadOnlyCollection<InstrumentUId> bondInstrumentIds, CancellationToken token)
    {
        var result = new List<Instrument>();
        foreach (var bondInstrumentId in bondInstrumentIds)
        {
            var command = await instrumentsProvider.BondProvide(bondInstrumentId, token);
            if (command == null)
                continue;

            var bondId = await bondsRepository.UpsertBond(command.Value, token);
            result.Add(new Instrument
            {
                InstrumentUId = bondInstrumentId,
                Id = bondId,
                Type = InstrumentType.Bond
            });
        }

        return result;
    }

    private async Task<IEnumerable<Instrument>> ImportCurrencies(IReadOnlyCollection<InstrumentUId> currencyInstrumentIds, CancellationToken token)
    {
        var result = new List<Instrument>();
        foreach (var currencyInstrumentId in currencyInstrumentIds)
        {
            var command = await instrumentsProvider.CurrencyProvide(currencyInstrumentId, token);
            if (command == null)
                continue;

            var currencyId = await currenciesRepository.UpsertCurrency(command.Value, token);
            result.Add(new Instrument
            {
                InstrumentUId = currencyInstrumentId,
                Id = currencyId,
                Type = InstrumentType.CurrencyAsset
            });
        }

        return result;
    }
}