using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement.Application.Services;

namespace Wealth.InstrumentManagement.Application.Tests.Instruments.Commands;

[TestFixture]
[TestOf(typeof(ChangePriceCommandHandler))]
public class ChangePriceCommandHandlerTests
{
    [Test]
    public async Task WhenHandle()
    {
        var instumentsRepository = A.Fake<IInstrumentsRepository>();
        var currencyService = A.Fake<ICurrencyService>();
        A.CallTo(() => currencyService.IsCurrencyExists("FOO")).Returns(true);
        var command = new ChangePriceCommand
        {
            Id = InstrumentId.New(),
            Price = new Money("FOO", 3.42m),
        };
        var handler = new ChangePriceCommandHandler(instumentsRepository, currencyService);
        
        await handler.Handle(command, CancellationToken.None);
        
        A.CallTo(() => currencyService.IsCurrencyExists(command.Price.CurrencyId)).MustHaveHappened();
        A.CallTo(() => instumentsRepository.ChangePrice(command.Id, command.Price)).MustHaveHappened();
    }
}