using Wealth.StrategyTracking.Application.Strategies.ComponentsProvider;
using Wealth.StrategyTracking.Domain.Strategies;
using System.Net.Http.Json;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement;

namespace Wealth.StrategyTracking.Infrastructure.Strategies;

public sealed class TinkoffMoexComponentProvider(
    InstrumentsService.InstrumentsServiceClient instrumentsServiceClient)
    : IMoexComponentsProvider
{
    private const string Url = "https://www.tinkoff.ru/api/invest-gw/capital/funds/v1/portfolio/structure?ticker=TMOS";
    private List<StrategyComponent>? cache;
    private readonly HttpClient _httpClient = new();

    public async Task<IReadOnlyCollection<StrategyComponent>> GetComponents(CancellationToken token)
    {
        if (cache != null)
            return cache;

        var response = await _httpClient.GetFromJsonAsync<Response>(Url, token);

        if (response?.Payload?.Instruments != null)
            cache = await BuildCache(response.Payload.Instruments, token);
        else
            throw new InvalidOperationException($"{nameof(TinkoffMoexComponentProvider)} returned null.");

        return cache;
    }

    private async Task<List<StrategyComponent>> BuildCache(List<Instrument> instruments, CancellationToken token)
    {
        var enriched = (await instrumentsServiceClient.GetInstrumentsByIsinAsync(new GetInstrumentsByIsinRequest
            {
                Isins =
                {
                    instruments
                        .Select(i => i.Isin)
                        .Where(i => i is not null)
                }
            }, cancellationToken: token))
            .Instruments.ToDictionary(i => i.Isin);

        return instruments.Select(BuildComponent).ToList();

        StrategyComponent BuildComponent(Instrument instrument)
        {
            if (StringComparer.OrdinalIgnoreCase.Equals(instrument.Type, "stock"))
            {
                if (instrument.Isin == null)
                    throw new InvalidOperationException($"{instrument.Name} stock isin is null.");

                var instrumentProto = enriched[instrument.Isin];
                return new StockStrategyComponent
                {
                    StockId = instrumentProto.Id,
                    Weight = instrument.RelativeValue,
                };
            }

            if (StringComparer.OrdinalIgnoreCase.Equals(instrument.Type, "currency"))
            {
                return new CurrencyStrategyComponent
                {
                    Currency = CurrencyCode.Rub,
                    Weight = instrument.RelativeValue,
                };
            }
            
            if (StringComparer.OrdinalIgnoreCase.Equals(instrument.Type, "bond"))
            {
                if (instrument.Isin == null)
                    throw new InvalidOperationException($"{instrument.Name} bond isin is null.");

                var instrumentProto = enriched[instrument.Isin];
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
        public string? Name { get; set; }
        public string? Ticker { get; set; }
        public string? Isin { get; set; }
        public string? Type { get; set; }
        public decimal RelativeValue { get; set; }
    }
}