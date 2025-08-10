using System.Text.Json;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc.Testing;
using Wealth.BuildingBlocks;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.DepositManagement.API.Tests;

public sealed class DepositManagementApiTests : IClassFixture<DepositManagementApiFixture>
{
    private readonly JsonSerializerOptions jsonSerializerOptions = new(JsonSerializerDefaults.Web);
    private readonly HttpClient httpClient;
    private readonly DepositsService.DepositsServiceClient client;

    public DepositManagementApiTests(DepositManagementApiFixture fixture)
    {
        WebApplicationFactory<Program> webApplicationFactory = fixture;
        httpClient = webApplicationFactory.CreateDefaultClient();

        var channel = GrpcChannel.ForAddress(
            httpClient.BaseAddress!,
            new GrpcChannelOptions
            {
                HttpClient = httpClient,
            });

        client = new DepositsService.DepositsServiceClient(channel);
    }

    [Fact]
    public async Task GetDeposits()
    {
        var seededDeposits = await client.GetDepositsAsync(new GetDepositsRequest());
        Assert.Equal(2, seededDeposits.Deposits.Count);
    }


    [Fact]
    public async Task WhenCreate()
    {
        var createDepositRequest = new CreateDepositRequest
        {
            Currency = "RUB",
            Name = "Test",
            Yield = new YieldProto
            {
                PercentPerYear = 0.23m
            }
        };

        var createResponse = await client.CreateDepositAsync(createDepositRequest);

        Assert.True(createResponse.DepositId.Id > 2);

        var createdDeposit = await GetDeposit(createResponse.DepositId);
        Assert.Equal(createDepositRequest.Name, createdDeposit.Name);
        Assert.Equal(createDepositRequest.Yield, createdDeposit.Yield);
        Assert.Equal(createDepositRequest.Currency, createdDeposit.InterestPerYear.CurrencyId);

        var investment = new Money("RUB", 32.32m);
        await client.InvestAsync(new InvestRequest { DepositId = createResponse.DepositId, Investment = investment });
        createdDeposit = await GetDeposit(createResponse.DepositId);
        Assert.Equal(investment, (Money)createdDeposit.Investment);
        
        var withdrawal = new Money("RUB", 20m);
        await client.WithdrawAsync(new WithdrawRequest { DepositId = createResponse.DepositId, Withdrawal = withdrawal });
        createdDeposit = await GetDeposit(createResponse.DepositId);
        Assert.Equal(investment - withdrawal, (Money)createdDeposit.Investment);
        
        async Task<DepositProto> GetDeposit(DepositId depositId) => (await client.GetDepositAsync(new GetDepositRequest { DepositId = depositId })).Deposit;
    }


    [Fact]
    public async Task WhenPortfolioNotFound()
    {
        var ex = await Assert.ThrowsAnyAsync<RpcException>(async () => await client.GetDepositAsync(new GetDepositRequest
        {
            DepositId = 123
        }));

        Assert.Equal(StatusCode.NotFound, ex.StatusCode);
    }
}