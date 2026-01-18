using System.Data;
using Dapper;
using Microsoft.Extensions.DependencyInjection;
using SharpJuice.Essentials;
using Wealth.BuildingBlocks.Infrastructure.Repositories;
using Wealth.InstrumentManagement.Application.Instruments.Commands;
using Wealth.InstrumentManagement.Application.Repositories;
using Wealth.InstrumentManagement.Infrastructure.Repositories;

namespace Wealth.InstrumentManagement.API.Tests.RepositoriesTests;

public sealed class InstrumentInitializer
{
    private readonly IStocksRepository stocksRepository;
    private readonly IBondsRepository bondsRepository;
    private readonly ICurrenciesRepository currenciesRepository;
    private readonly IConnectionFactory connectionFactory;

    public InstrumentInitializer(InstrumentManagementApiFixture apiFixture)
    {
        connectionFactory = apiFixture.Services.GetRequiredService<IConnectionFactory>();
        var scope = apiFixture.Services.CreateScope();
        stocksRepository = scope.ServiceProvider.GetRequiredService<IStocksRepository>();
        bondsRepository = scope.ServiceProvider.GetRequiredService<IBondsRepository>();
        currenciesRepository = scope.ServiceProvider.GetRequiredService<ICurrenciesRepository>();
    }

    public async Task CreateStocks(IReadOnlyCollection<CreateStockCommand> stocks)
    {
        foreach (var stock in stocks)
            await stocksRepository.CreateStock(stock, CancellationToken.None);
    }

    public async Task CreateBonds(IReadOnlyCollection<CreateBondCommand> bonds)
    {
        foreach (var bond in bonds)
            await bondsRepository.CreateBond(bond, CancellationToken.None);
    }

    public async Task CreateCurrencies(IReadOnlyCollection<CreateCurrencyCommand> currencies)
    {
        foreach (var currency in currencies)
            await currenciesRepository.CreateCurrency(currency, CancellationToken.None);
    }

    public Task Clear()
    {
        var connection = connectionFactory.CreateConnection();
        if (connection.State != ConnectionState.Open)
            connection.Open();

        var command = new CommandDefinition(
            // language=postgresql
            """
            TRUNCATE TABLE "Stocks";
            TRUNCATE TABLE "Bonds";
            TRUNCATE TABLE currencies;
            """);

        return connection.ExecuteAsync(command);
    }
}