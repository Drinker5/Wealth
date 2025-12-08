using System.Text.Json;
using AutoFixture;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc.Testing;
using Wealth.BuildingBlocks;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.StrategyTracking.API.Tests;

public sealed class StrategiesGrpcApiTests : IClassFixture<StrategyTrackingApiFixture>
{
    private readonly Fixture fixture = new Fixture();
    private readonly StrategiesService.StrategiesServiceClient grpcClient;

    public StrategiesGrpcApiTests(StrategyTrackingApiFixture apiFixture)
    {
        WebApplicationFactory<Program> webApplicationFactory = apiFixture;
        var httpClient = webApplicationFactory.CreateDefaultClient();
        var channel = GrpcChannel.ForAddress(
            httpClient.BaseAddress!,
            new GrpcChannelOptions
            {
                HttpClient = httpClient,
            });

        grpcClient = new StrategiesService.StrategiesServiceClient(channel);
    }

    [Fact]
    public async Task GetStrategy_AsExpected()
    {
        var response = await grpcClient.GetStrategyAsync(new GetStrategyRequest { StrategyId = 1 });

        Assert.NotNull(response);
        Assert.Equal("Seed-strategy-1", response.Name);
        Assert.Collection(response.Components,
            c1 =>
            {
                Assert.Equal(50, c1.Weight);
                Assert.Equal(1, c1.InstrumentId);
                Assert.Equal(InstrumentTypeProto.Stock, c1.InstrumentType);
            },
            c2 =>
            {
                Assert.Equal(30, c2.Weight);
                Assert.Equal(1, c2.InstrumentId);
                Assert.Equal(InstrumentTypeProto.Bond, c2.InstrumentType);
            },
            c3 =>
            {
                Assert.Equal(12, c3.Weight);
                Assert.Equal(1, c3.InstrumentId);
                Assert.Equal(InstrumentTypeProto.CurrencyAsset, c3.InstrumentType);
            },
            c4 =>
            {
                Assert.Equal(8, c4.Weight);
                Assert.Equal((int)CurrencyCode.Cny, c4.InstrumentId);
                Assert.Equal(InstrumentTypeProto.Currency, c4.InstrumentType);
            });
    }
}