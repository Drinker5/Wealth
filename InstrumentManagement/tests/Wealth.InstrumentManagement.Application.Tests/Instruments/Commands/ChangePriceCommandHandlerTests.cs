using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement.Application.Instruments.Commands;
using Wealth.InstrumentManagement.Application.Services;
using Wealth.InstrumentManagement.Domain.Repositories;

namespace Wealth.InstrumentManagement.Application.Tests.Instruments.Commands;

[TestFixture]
[TestOf(typeof(ChangeStockPriceCommandHandler))]
public class ChangePriceCommandHandlerTests
{
    [Test]
    public async Task WhenHandle()
    {
        var instumentsRepository = A.Fake<IStocksRepository>();
        var currencyService = A.Fake<ICurrencyService>();
        A.CallTo(() => currencyService.IsCurrencyExists(CurrencyCode.RUB)).Returns(true);
        var command = new ChangeStockPriceCommand
        {
            StockId = new StockId(3),
            Price = new Money(CurrencyCode.RUB, 3.42m),
        };
        var handler = new ChangeStockPriceCommandHandler(instumentsRepository, currencyService);
        
        await handler.Handle(command, CancellationToken.None);
        
        A.CallTo(() => currencyService.IsCurrencyExists(command.Price.CurrencyId)).MustHaveHappened();
        A.CallTo(() => instumentsRepository.ChangePrice(command.StockId, command.Price)).MustHaveHappened();
    }
}