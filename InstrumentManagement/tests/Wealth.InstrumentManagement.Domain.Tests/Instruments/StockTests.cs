using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement.Domain.Instruments;
using Wealth.InstrumentManagement.Domain.Instruments.Events;

namespace Wealth.InstrumentManagement.Domain.Tests.Instruments;

[TestFixture]
[TestOf(typeof(Stock))]
public class StockTests
{
    readonly string name = "foo";
    readonly ISIN isin = "barbarbarbar";
    readonly FIGI figi = "arbarbarbarb";
    readonly Dividend dividend = new Dividend(CurrencyCode.Rub, Decimal.One);

    private Stock CreateStockInstrument(string name, ISIN isin, FIGI figi)
    {
        return Stock.Create(3, name, isin, figi);
    }

    [Test]
    public void WhenCreate()
    {
        var stock = CreateStockInstrument(name, isin, figi);

        var @event = stock.HasEvent<StockCreated>();
        using (Assert.EnterMultipleScope())
        {
            Assert.That(@event.StockId, Is.Not.Zero);
            Assert.That(@event.Name, Is.EqualTo(name));
            Assert.That(@event.Isin, Is.EqualTo(isin));
            Assert.That(stock.Id, Is.Not.Zero);
            Assert.That(stock.Name, Is.EqualTo(name));
            Assert.That(stock.Isin, Is.EqualTo(isin));
            Assert.That(stock.Dividend, Is.EqualTo(Dividend.Empty));
            Assert.That(stock.LotSize.Value, Is.EqualTo(1));
        }
    }

    [Test]
    public void WhenPriceChanged()
    {
        var stock = CreateStockInstrument(name, isin, figi);
        var money = new Money(CurrencyCode.Eur, 23.3m);

        stock.ChangePrice(money);

        var @event = stock.HasEvent<StockPriceChanged>();
        using (Assert.EnterMultipleScope())
        {
            Assert.That(@event.StockId, Is.Not.Zero);
            Assert.That(@event.ISIN, Is.EqualTo(isin));
            Assert.That(@event.NewPrice, Is.EqualTo(money));
        }
    }

    [Test]
    public void WhenDividendChanged()
    {
        var stock = CreateStockInstrument(name, isin, figi);

        stock.ChangeDividend(dividend);

        var @event = stock.HasEvent<StockDividendChanged>();
        using (Assert.EnterMultipleScope())
        {
            Assert.That(@event.StockId, Is.Not.Zero);
            Assert.That(@event.ISIN, Is.EqualTo(isin));
            Assert.That(@event.NewDividend, Is.EqualTo(dividend));
        }
    }

    [Test]
    public void WhenLotSizeChanged()
    {
        var instrument = CreateStockInstrument(name, isin, figi);

        instrument.ChangeLotSize(10);

        var @event = instrument.HasEvent<StockLotSizeChanged>();
        using (Assert.EnterMultipleScope())
        {
            Assert.That(@event.StockId, Is.Not.Zero);
            Assert.That(@event.ISIN, Is.EqualTo(isin));
            Assert.That(@event.NewLotSize, Is.EqualTo(10));
        }
    }
}