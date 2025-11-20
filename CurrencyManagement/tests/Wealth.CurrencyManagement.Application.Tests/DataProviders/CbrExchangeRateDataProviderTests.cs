using System.Net;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.CurrencyManagement.Infrastructure.DataProviders;

namespace Wealth.CurrencyManagement.Application.Tests.DataProviders;

[TestSubject(typeof(CbrExchangeRateDataProvider))]
public class CbrExchangeRateDataProviderTests
{
    private readonly CbrExchangeRateDataProvider provider;
    private const CurrencyCode USD = CurrencyCode.Usd;
    private const CurrencyCode RUB = CurrencyCode.Rub;
    private readonly DateOnly date = new DateOnly(2020, 1, 1);
    private readonly MockHttpMessageHandler handler;
    private readonly HttpResponseMessage httpResponse;

    public CbrExchangeRateDataProviderTests()
    {
        var logger = Substitute.For<ILogger<CbrExchangeRateDataProvider>>();
        handler = Substitute.ForPartsOf<MockHttpMessageHandler>();
        var httpClient = new HttpClient(handler);
        httpResponse = new HttpResponseMessage();
        handler.MockSend(Arg.Any<HttpRequestMessage>(), Arg.Any<CancellationToken>()).Returns(httpResponse);

        provider = new CbrExchangeRateDataProvider(
            logger,
            httpClient,
            Substitute.For<IMemoryCache>());
    }

    [Fact]
    public async Task WhenGetRate_OnlyRUBAsTarget()
    {
        await Assert.ThrowsAnyAsync<ArgumentException>(() =>
            provider.GetRate(RUB, USD, date)
        );
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public async Task WhenGetRate(int _)
    {
        var xml = await File.ReadAllTextAsync("DataProviders/response.xml");
        httpResponse.Content = new StringContent(xml);
        httpResponse.StatusCode = HttpStatusCode.OK;

        var result = await provider.GetRate(USD, RUB, date);

        handler.Received(1).MockSend(Arg.Any<HttpRequestMessage>(), Arg.Any<CancellationToken>());
        Assert.Equal(96.4706m, result);
    }

    public class MockHttpMessageHandler : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(MockSend(request, cancellationToken));
        }

        protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return MockSend(request, cancellationToken);
        }

        public virtual HttpResponseMessage MockSend(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}