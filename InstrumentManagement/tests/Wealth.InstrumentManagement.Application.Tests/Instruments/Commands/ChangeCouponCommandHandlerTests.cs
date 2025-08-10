using Wealth.InstrumentManagement.Application.Instruments.Commands;
using Wealth.InstrumentManagement.Application.Services;
using Wealth.InstrumentManagement.Domain.Instruments;
using Wealth.InstrumentManagement.Domain.Repositories;

namespace Wealth.InstrumentManagement.Application.Tests.Instruments.Commands;

[TestFixture]
[TestOf(typeof(ChangeCouponCommandHandler))]
public class ChangeCouponCommandHandlerTests
{
    [Test]
    public async Task WhenHandle()
    {
        var bondsRepository = A.Fake<IBondsRepository>();
        var currencyService = A.Fake<ICurrencyService>();
        A.CallTo(() => currencyService.IsCurrencyExists("FOO")).Returns(true);
        var command = new ChangeCouponCommand
        {
            Id = InstrumentId.New(),
            Coupon = new Coupon("FOO", 3.42m),
        };
        var handler = new ChangeCouponCommandHandler(bondsRepository, currencyService);
        
        await handler.Handle(command, CancellationToken.None);
        
        A.CallTo(() => currencyService.IsCurrencyExists(command.Coupon.ValuePerYear.CurrencyId)).MustHaveHappened();
        A.CallTo(() => bondsRepository.ChangeCoupon(command.Id, command.Coupon)).MustHaveHappened();
    }
}