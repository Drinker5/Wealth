using System.Data;
using Dapper;
using Microsoft.Extensions.DependencyInjection;
using Wealth.BuildingBlocks.Infrastructure.Repositories;
using Wealth.InstrumentManagement.Application.Instruments.Commands;
using Wealth.InstrumentManagement.Infrastructure.Repositories;

namespace Wealth.InstrumentManagement.API.Tests.RepositoriesTests;

public sealed class InstrumentInitializer
{
    private readonly StocksRepository stocksRepository;
    private readonly BondsRepository bondsRepository;
    private readonly CurrenciesRepository currenciesRepository;
    private readonly IConnectionFactory connectionFactory;

    public InstrumentInitializer(InstrumentManagementApiFixture apiFixture)
    {
        connectionFactory = apiFixture.Services.GetRequiredService<IConnectionFactory>();
        var eventTracker = apiFixture.Services.GetRequiredService<IEventTracker>();
        stocksRepository = new StocksRepository(connectionFactory, eventTracker);
        bondsRepository = new BondsRepository(connectionFactory, eventTracker);
        currenciesRepository = new CurrenciesRepository(connectionFactory, eventTracker);
    }

    public async Task CreateStocks(IReadOnlyCollection<CreateStockCommand> stocks)
    {
        foreach (var stock in stocks)
            await stocksRepository.CreateStock(stock);
    }

    public async Task CreateBonds(IReadOnlyCollection<CreateBondCommand> bonds)
    {
        foreach (var bond in bonds)
            await bondsRepository.CreateBond(bond);
    }

    public async Task CreateCurrencies(IReadOnlyCollection<CreateCurrencyCommand> currencies)
    {
        foreach (var currency in currencies)
            await currenciesRepository.CreateCurrency(currency);
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