using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement.Application.Instruments.Commands;
using Wealth.InstrumentManagement.Application.Repositories;
using Wealth.InstrumentManagement.Domain.Instruments;

namespace Wealth.InstrumentManagement.Application.Tests.Instruments.Commands;

[TestFixture]
[TestOf(typeof(ChangeDividendCommandHandler))]
public class ChangeDividendCommandHandlerTests
{
    [Test]
    public async Task WhenHandle()
    {
        var stocksRepository = A.Fake<IStocksRepository>();
        var command = new ChangeDividendCommand
        {
            StockId = new StockId(3),
            Dividend = new Dividend(CurrencyCode.Rub, 3.42m),
        };
        var handler = new ChangeDividendCommandHandler(stocksRepository);
        
        await handler.Handle(command, CancellationToken.None);
        
        A.CallTo(() => stocksRepository.ChangeDividend(command.StockId, command.Dividend)).MustHaveHappened();
    }
}