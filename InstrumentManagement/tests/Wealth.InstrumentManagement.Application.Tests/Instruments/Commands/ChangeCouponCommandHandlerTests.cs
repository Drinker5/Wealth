using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement.Application.Instruments.Commands;
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
        var command = new ChangeCouponCommand
        {
            Id = new BondId(3),
            Coupon = new Coupon(CurrencyCode.Rub, 3.42m),
        };
        var handler = new ChangeCouponCommandHandler(bondsRepository);
        
        await handler.Handle(command, CancellationToken.None);
        
        A.CallTo(() => bondsRepository.ChangeCoupon(command.Id, command.Coupon)).MustHaveHappened();
    }
}