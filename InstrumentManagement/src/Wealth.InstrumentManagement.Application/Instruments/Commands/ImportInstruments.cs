using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement.Application.Instruments.Models;
using Wealth.InstrumentManagement.Application.Providers;
using Wealth.InstrumentManagement.Application.Repositories;

namespace Wealth.InstrumentManagement.Application.Instruments.Commands;

public readonly record struct ImportInstruments(
    IReadOnlyCollection<InstrumentId> StockInstrumentIds,
    IReadOnlyCollection<InstrumentId> BondInstrumentIds,
    IReadOnlyCollection<InstrumentId> CurrencyInstrumentIds) : ICommand<IReadOnlyCollection<Instrument>>;

public sealed class UpdateInstrumentsHandler(
    IInstrumentsProvider instrumentsProvider,
    IInstrumentsRepository repository,
    IStocksRepository stocksRepository,
    IBondsRepository bondsRepository,
    ICurrenciesRepository currenciesRepository)
    : ICommandHandler<ImportInstruments, IReadOnlyCollection<Instrument>>
{
    public async Task<IReadOnlyCollection<Instrument>> Handle(ImportInstruments request, CancellationToken token)
    {
        var stockInstrumentIds = request.StockInstrumentIds.ToHashSet();
        var bondInstrumentIds = request.BondInstrumentIds.ToHashSet();
        var currencyInstrumentIds = request.CurrencyInstrumentIds.ToHashSet();
        var instrumentIds = stockInstrumentIds
            .Concat(bondInstrumentIds)
            .Concat(currencyInstrumentIds)
            .ToArray();

        var instruments = (await repository.GetInstruments(instrumentIds, token)).ToList();
        foreach (var instrument in instruments)
        {
            stockInstrumentIds.Remove(instrument.InstrumentId);
            bondInstrumentIds.Remove(instrument.InstrumentId);
            currencyInstrumentIds.Remove(instrument.InstrumentId);
        }

        var result = new List<Instrument>(stockInstrumentIds.Count + bondInstrumentIds.Count + currencyInstrumentIds.Count);
        result.AddRange(instruments);
        if (stockInstrumentIds.Count > 0)
            result.AddRange(await ImportStocks(stockInstrumentIds, token));
        if (bondInstrumentIds.Count > 0)
            result.AddRange(await ImportBonds(bondInstrumentIds, token));
        if (currencyInstrumentIds.Count > 0)
            result.AddRange(await ImportCurrencies(currencyInstrumentIds, token));

        return result;
    }

    private async Task<IEnumerable<Instrument>> ImportStocks(IReadOnlyCollection<InstrumentId> stockInstrumentIds, CancellationToken token)
    {
        var result = new List<Instrument>();
        foreach (var stockInstrumentId in stockInstrumentIds)
        {
            var stockProvideCommand = await instrumentsProvider.StockProvide(stockInstrumentId, token);
            var stockId = await stocksRepository.UpsertStock(stockProvideCommand, token);
            result.Add(new Instrument
            {
                InstrumentId = stockInstrumentId,
                Id = stockId,
                Type = InstrumentType.Stock
            });
        }

        return result;
    }

    private async Task<IEnumerable<Instrument>> ImportBonds(IReadOnlyCollection<InstrumentId> bondInstrumentIds, CancellationToken token)
    {
        var result = new List<Instrument>();
        foreach (var bondInstrumentId in bondInstrumentIds)
        {
            var bondProvideCommand = await instrumentsProvider.BondProvide(bondInstrumentId, token);
            // TODO Upsert
            var bondId = await bondsRepository.CreateBond(bondProvideCommand, token);
            result.Add(new Instrument
            {
                InstrumentId = bondInstrumentId,
                Id = bondId,
                Type = InstrumentType.Bond
            });
        }

        return result;
    }
    
    private async Task<IEnumerable<Instrument>> ImportCurrencies(IReadOnlyCollection<InstrumentId> currencyInstrumentIds, CancellationToken token)
    {
        var result = new List<Instrument>();
        foreach (var currencyInstrumentId in currencyInstrumentIds)
        {
            var currencyProvideCommand = await instrumentsProvider.CurrencyProvide(currencyInstrumentId, token);
            // TODO Upsert
            var currencyId = await currenciesRepository.CreateCurrency(currencyProvideCommand, token);
            result.Add(new Instrument
            {
                InstrumentId = currencyInstrumentId,
                Id = currencyId,
                Type = InstrumentType.CurrencyAsset
            });
        }

        return result;
    }
}