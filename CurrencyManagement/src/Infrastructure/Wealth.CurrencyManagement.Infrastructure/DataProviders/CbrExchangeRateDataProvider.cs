using System.Globalization;
using System.Threading.RateLimiting;
using System.Xml.Linq;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Bulkhead;
using Polly.CircuitBreaker;
using Polly.RateLimit;
using Polly.Retry;
using Wealth.CurrencyManagement.Application.DataProviders;
using Wealth.CurrencyManagement.Domain.Currencies;
using Wealth.CurrencyManagement.Domain.Utilities;

namespace Wealth.CurrencyManagement.Infrastructure.DataProviders;

public class CbrExchangeRateDataProvider : IExchangeRateDataProvider
{
    private readonly ILogger<CbrExchangeRateDataProvider> logger;
    private readonly HttpClient httpClient;
    private readonly IMemoryCache cache;
    private static readonly CultureInfo CultureInfo = CultureInfo.GetCultureInfo("ru-RU");
    private const string BaseUrl = "https://www.cbr.ru/scripts/XML_daily.asp";
    private static readonly TimeSpan cacheDuration = TimeSpan.FromMinutes(5);
    private static readonly ResiliencePipeline pipeline = CreateResiliencePipeline();

    public CbrExchangeRateDataProvider(
        ILogger<CbrExchangeRateDataProvider> logger,
        HttpClient httpClient,
        IMemoryCache cache)
    {
        this.logger = logger;
        this.httpClient = httpClient;
        this.cache = cache;
    }

    public async Task<decimal> GetRate(CurrencyId baseCurrencyId, CurrencyId targetCurrencyId, DateOnly? validOnDate)
    {
        if (targetCurrencyId != "RUB")
            throw new ArgumentException("Only RUB targetCurrencyId is supported");

        if (baseCurrencyId == targetCurrencyId)
            return 1;

        validOnDate ??= Clock.Today;

        var currencyCode = baseCurrencyId.Code;
        var cacheKey = $"{currencyCode}_{validOnDate:dd.MM.yyyy}";
        if (cache.TryGetValue(cacheKey, out decimal cachedRate))
            return cachedRate;

        if (cache.TryGetValue(cacheKey, out cachedRate))
            return cachedRate;

        var valueString = await RequestValute(validOnDate.Value, currencyCode, CancellationToken.None);
        if (string.IsNullOrEmpty(valueString))
            throw new InvalidOperationException($"No Valute found for {currencyCode}");

        var rate = Parse(valueString);
        cache.Set(cacheKey, rate, cacheDuration);
        return rate;
    }

    private static decimal Parse(string valueString)
    {
        return decimal.Parse(valueString, CultureInfo);
    }

    //<Valute ID="R01010">
    //  <NumCode>036</NumCode>
    //  <CharCode>AUD</CharCode>
    //  <Nominal>1</Nominal>
    //  <Name>Австралийский доллар</Name>
    //  <Value>50,9632</Value>
    //  <VunitRate>50,9632</VunitRate>
    //</Valute>
    private async Task<string?> RequestValute(DateOnly validOnDate, string currencyCode, CancellationToken token)
    {
        var url = $"{BaseUrl}?date_req={validOnDate:dd.MM.yyyy}";
        try
        {
            var response = await pipeline.ExecuteAsync(async (client, ct) => await client.GetAsync(url, ct), httpClient, token);
            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException($"HTTP error: {response.StatusCode}");

            var content = await response.Content.ReadAsStringAsync(token);
            var xdoc = XDocument.Parse(content);
            var valute = xdoc.Descendants("Valute")
                .FirstOrDefault(v => v.Element("CharCode")?.Value == currencyCode);

            return valute?.Element("VunitRate")?.Value;
        }
        catch (BulkheadRejectedException)
        {
            logger.LogWarning("Request queue is full. Try again later.");
        }
        catch (RateLimitRejectedException ex)
        {
            logger.LogWarning("Rate limit exceeded. Retry after {ExRetryAfter}", ex.RetryAfter);
        }
        catch (BrokenCircuitException)
        {
            logger.LogWarning("Service temporarily unavailable (Circuit Breaker)");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Request failed");
        }

        return null;
    }

    private static ResiliencePipeline CreateResiliencePipeline()
    {
        return new ResiliencePipelineBuilder()
            .AddRateLimiter(new SlidingWindowRateLimiter(
                new SlidingWindowRateLimiterOptions()
                {
                    PermitLimit = 1,
                    SegmentsPerWindow = 1,
                    QueueLimit = 10,
                    Window = TimeSpan.FromSeconds(1)
                }))
            .AddCircuitBreaker(
                new CircuitBreakerStrategyOptions
                {
                    BreakDuration = TimeSpan.FromMinutes(1)
                })
            .AddRetry(
                new RetryStrategyOptions
                {
                    MaxRetryAttempts = 3,
                    DelayGenerator = DelayGenerator,
                })
            .Build();

        ValueTask<TimeSpan?> DelayGenerator(RetryDelayGeneratorArguments<object> arguments)
        {
            return ValueTask.FromResult<TimeSpan?>(TimeSpan.FromSeconds(Math.Pow(2, arguments.AttemptNumber)));
        }
    }
}