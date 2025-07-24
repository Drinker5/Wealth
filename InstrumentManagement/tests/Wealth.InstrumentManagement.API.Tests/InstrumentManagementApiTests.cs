using System.Text.Json;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc.Testing;
using Wealth.BuildingBlocks;
using Xunit;
using Assert = Xunit.Assert;

namespace Wealth.InstrumentManagement.API.Tests;

public sealed class InstrumentManagementApiTests : IClassFixture<InstrumentManagementApiFixture>
{
    private readonly JsonSerializerOptions jsonSerializerOptions = new(JsonSerializerDefaults.Web);
    private readonly InstrumentsService.InstrumentsServiceClient client;

    public InstrumentManagementApiTests(InstrumentManagementApiFixture fixture)
    {
        WebApplicationFactory<Program> webApplicationFactory = fixture;
        var httpClient = webApplicationFactory.CreateDefaultClient();
        var channel = GrpcChannel.ForAddress(
            httpClient.BaseAddress!,
            new GrpcChannelOptions
            {
                HttpClient = httpClient,
            });

        client = new InstrumentsService.InstrumentsServiceClient(channel);
    }

    [Fact]
    public async Task GetInstruments()
    {
        var instrumentId = new InstrumentIdProto
        {
            Id = new Guid("00000000-0000-0000-0000-000000000001"),
        };

        var instrument = await client.GetInstrumentAsync(new GetInstrumentRequest
        {
            Id = instrumentId
        });

        Assert.Equal(GetInstrumentResponse.InstrumentOneofCase.BondInfo, instrument.InstrumentCase);
        Assert.Equal("test-bond-1", instrument.Name);
    }
}