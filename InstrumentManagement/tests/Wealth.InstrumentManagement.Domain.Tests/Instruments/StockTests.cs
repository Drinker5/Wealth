using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement.Domain.Instruments;
using Wealth.InstrumentManagement.Domain.Instruments.Events;

namespace Wealth.InstrumentManagement.Domain.Tests.Instruments;

[TestFixture]
[TestOf(typeof(Stock))]
public class StockTests
{
    readonly string index = "bar";
    readonly string name = "foo";
    readonly ISIN isin = "barbarbarbar";
    readonly FIGI figi = "arbarbarbarb";
    readonly InstrumentUId instrumentUId = new Guid("FCFF748D-C4E9-4388-B9B0-6164AB55E428");
    readonly Dividend dividend = new Dividend(CurrencyCode.Rub, Decimal.One);

    private Stock CreateStockInstrument()
    {
        return Stock.Create(3, index, name, isin, figi, instrumentUId, CurrencyCode.Rub);
    }

    [Test]
    public void WhenCreate()
    {
        var stock = CreateStockInstrument();

        var @event = stock.HasEvent<StockCreated>();
        using (Assert.EnterMultipleScope())
        {
            Assert.That(@event.StockId, Is.Not.Zero);
            Assert.That(@event.Index, Is.EqualTo(index));
            Assert.That(@event.Name, Is.EqualTo(name));
            Assert.That(@event.Isin, Is.EqualTo(isin));
            Assert.That(@event.InstrumentUId, Is.EqualTo(instrumentUId));
            Assert.That(stock.Id, Is.Not.Zero);
            Assert.That(stock.Name, Is.EqualTo(name));
            Assert.That(stock.Isin, Is.EqualTo(isin));
            Assert.That(stock.InstrumentUId, Is.EqualTo(instrumentUId));
            Assert.That(stock.Dividend, Is.EqualTo(Dividend.Empty));
            Assert.That(stock.LotSize.Value, Is.EqualTo(1));
        }
    }

    [Test]
    public void WhenPriceChanged()
    {
        var stock = CreateStockInstrument();
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
        var stock = CreateStockInstrument();

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
        var instrument = CreateStockInstrument();

        instrument.ChangeLotSize(10);

        var @event = instrument.HasEvent<StockLotSizeChanged>();
        using (Assert.EnterMultipleScope())
        {
            Assert.That(@event.StockId, Is.Not.Zero);
            Assert.That(@event.ISIN, Is.EqualTo(isin));
            Assert.That(@event.NewLotSize, Is.EqualTo(10));
        }
    }
    
    [Test]
    public void WhenIndexChanged()
    {
        var instrument = CreateStockInstrument();

        instrument.ChangeTicker("qwe");

        var @event = instrument.HasEvent<StockTickerChanged>();
        using (Assert.EnterMultipleScope())
        {
            Assert.That(@event.StockId, Is.Not.Zero);
            Assert.That(@event.NewTicker, Is.EqualTo("qwe"));
        }
    }
}