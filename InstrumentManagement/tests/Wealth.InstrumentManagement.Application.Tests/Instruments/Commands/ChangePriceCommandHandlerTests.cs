using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement.Application.Instruments.Commands;
using Wealth.InstrumentManagement.Domain.Repositories;

namespace Wealth.InstrumentManagement.Application.Tests.Instruments.Commands;

[TestFixture]
[TestOf(typeof(ChangeStockPriceCommandHandler))]
public class ChangePriceCommandHandlerTests
{
    [Test]
    public async Task WhenHandle()
    {
        var instrumentsRepository = A.Fake<IStocksRepository>();
        var command = new ChangeStockPriceCommand
        {
            StockId = new StockId(3),
            Price = new Money(CurrencyCode.Rub, 3.42m),
        };
        var handler = new ChangeStockPriceCommandHandler(instrumentsRepository);
        
        await handler.Handle(command, CancellationToken.None);
        
        A.CallTo(() => instrumentsRepository.ChangePrice(command.StockId, command.Price)).MustHaveHappened();
    }
}