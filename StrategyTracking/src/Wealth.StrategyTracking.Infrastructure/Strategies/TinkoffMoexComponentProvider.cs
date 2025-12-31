using Wealth.StrategyTracking.Application.Strategies.ComponentsProvider;
using Wealth.StrategyTracking.Domain.Strategies;
using System.Net.Http.Json;
using Wealth.BuildingBlocks;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement;

namespace Wealth.StrategyTracking.Infrastructure.Strategies;

public sealed class TinkoffMoexComponentProvider(
    InstrumentsService.InstrumentsServiceClient instrumentsServiceClient)
    : IMoexComponentsProvider
{
    private const string Url = "https://www.tinkoff.ru/api/invest-gw/capital/funds/v1/portfolio/structure?ticker=TMOS";
    private const string Stock = "stock";
    private const string Bond = "bond";
    private const string? Currency = "currency";
    private readonly HttpClient _httpClient = new();

    public async Task<IReadOnlyCollection<StrategyComponent>> GetComponents(CancellationToken token)
    {
        var response = await _httpClient.GetFromJsonAsync<Response>(Url, token);

        if (response?.Payload?.Instruments != null)
            return await Build(response.Payload.Instruments, token);

        throw new InvalidOperationException($"{nameof(TinkoffMoexComponentProvider)} returned null.");
    }

    private async Task<List<StrategyComponent>> Build(List<Instrument> strategyInstruments, CancellationToken token)
    {
        var strategyInstrumentIds = strategyInstruments
            .Where(i => i.InstrumentUID is not null)
            .ToDictionary(i => InstrumentUId.From(i.InstrumentUID!));

        var instrumentsResponse = await instrumentsServiceClient.GetInstrumentsAsync(new GetInstrumentsRequest
        {
            InstrumentIds = { strategyInstrumentIds.Select(i => new InstrumentIdProto(i.Key)) }
        }, cancellationToken: token);

        var instruments = instrumentsResponse.Instruments.ToDictionary(i => (InstrumentUId)i.InstrumentId);
        if (strategyInstrumentIds.Count != instruments.Count)
        {
            foreach (var id in instruments.Keys)
                strategyInstrumentIds.Remove(id);

            if (strategyInstrumentIds.Count > 0)
            {
                var g = strategyInstrumentIds.Values
                    .GroupBy(i => i.Type)
                    .ToDictionary(k => k.Key,
                        v => v.Select(i => new InstrumentIdProto(i.InstrumentUID!)),
                        StringComparer.OrdinalIgnoreCase);

                var request = new ImportInstrumentsRequest
                {
                    StockInstrumentIds = { g.TryGetValue(Stock, out var stocks) ? stocks : [] },
                    BondInstrumentIds = { g.TryGetValue(Bond, out var bonds) ? bonds : [] },
                    CurrencyInstrumentIds = { }
                };

                var updateInstrumentsResponse = await instrumentsServiceClient.ImportInstrumentsAsync(
                    request, cancellationToken: token);

                if (strategyInstrumentIds.Count != updateInstrumentsResponse.Instruments.Count)
                {
                    foreach (var instrument in updateInstrumentsResponse.Instruments)
                        strategyInstrumentIds.Remove(instrument.InstrumentId);

                    throw new InvalidOperationException($"Did not find {string.Join(", ", strategyInstrumentIds)} instruments.");
                }
            }
        }

        return strategyInstruments.Select(BuildComponent).ToList();

        StrategyComponent BuildComponent(Instrument instrument)
        {
            if (StringComparer.OrdinalIgnoreCase.Equals(instrument.Type, Stock))
            {
                if (instrument.InstrumentUID == null)
                    throw new InvalidOperationException($"{instrument.InstrumentUID} stock uid is null.");

                var instrumentProto = instruments[instrument.InstrumentUID];
                return new StockStrategyComponent
                {
                    StockId = instrumentProto.Id,
                    Weight = instrument.RelativeValue,
                };
            }

            if (StringComparer.OrdinalIgnoreCase.Equals(instrument.Type, Currency))
            {
                return new CurrencyStrategyComponent
                {
                    Currency = CurrencyCode.Rub,
                    Weight = instrument.RelativeValue,
                };
            }

            if (StringComparer.OrdinalIgnoreCase.Equals(instrument.Type, Bond))
            {
                if (instrument.InstrumentUID == null)
                    throw new InvalidOperationException($"{instrument.Name} bond uid is null.");

                var instrumentProto = instruments[instrument.InstrumentUID];
                return new BondStrategyComponent
                {
                    BondId = instrumentProto.Id,
                    Weight = instrument.RelativeValue,
                };
            }

            throw new ArgumentOutOfRangeException($"{instrument.Type} is not handled.");
        }
    }

    private sealed class Response
    {
        public Payload? Payload { get; set; }
    }

    private sealed class Payload
    {
        public List<Instrument>? Instruments { get; set; }
    }

    private sealed class Instrument
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public decimal RelativeValue { get; set; }
        public string? Isin { get; set; }
        public string? Ticker { get; set; }
        public string? InstrumentUID { get; set; }
    }
}