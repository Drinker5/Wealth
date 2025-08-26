using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement.Application.Instruments.Commands;
using Wealth.InstrumentManagement.Application.Services;
using Wealth.InstrumentManagement.Domain.Instruments;
using Wealth.InstrumentManagement.Domain.Repositories;

namespace Wealth.InstrumentManagement.Application.Tests.Instruments.Commands;

[TestFixture]
[TestOf(typeof(ChangeDividendCommandHandler))]
public class ChangeDividendCommandHandlerTests
{
    [Test]
    public async Task WhenHandle()
    {
        var stocksRepository = A.Fake<IStocksRepository>();
        var currencyService = A.Fake<ICurrencyService>();
        A.CallTo(() => currencyService.IsCurrencyExists("FOO")).Returns(true);
        var command = new ChangeDividendCommand
        {
            Id = new StockId(3),
            Dividend = new Dividend("FOO", 3.42m),
        };
        var handler = new ChangeDividendCommandHandler(stocksRepository, currencyService);
        
        await handler.Handle(command, CancellationToken.None);
        
        A.CallTo(() => currencyService.IsCurrencyExists(command.Dividend.ValuePerYear.CurrencyId)).MustHaveHappened();
        A.CallTo(() => stocksRepository.ChangeDividend(command.Id, command.Dividend)).MustHaveHappened();
    }
}