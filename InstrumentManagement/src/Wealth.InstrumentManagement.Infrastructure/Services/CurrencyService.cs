using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement.Application.Services;

namespace Wealth.InstrumentManagement.Infrastructure.Services;

public class CurrencyService(ILogger<CurrencyService> logger, HttpClient httpClient) : ICurrencyService
{
    private readonly string remoteServiceBaseUrl = "api/currency";

    public async Task<bool> IsCurrencyExists(CurrencyId currencyId)
    {
        try
        {
            var currency = await GetCurrency(currencyId);
            return currency != null;
        }
        catch
        {
            return false;
        }
    }

    private async Task<CurrencyItem?> GetCurrency(CurrencyId id)
    {
        var uri = $"{remoteServiceBaseUrl}/{id.Value}";
        var response = await httpClient.GetAsync(uri);

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return await response.Content.ReadFromJsonAsync<CurrencyItem>();
        }

        var errorContent = await response.Content.ReadAsStringAsync();
        logger.LogError($"Error fetching currency {id.Value}. Status: {response.StatusCode}. Content: {errorContent}");
        return null;
    }
}