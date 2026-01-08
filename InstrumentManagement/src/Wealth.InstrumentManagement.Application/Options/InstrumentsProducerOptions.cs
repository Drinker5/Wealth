using System.ComponentModel.DataAnnotations;

namespace Wealth.InstrumentManagement.Application.Options;

public sealed class InstrumentsProducerOptions
{
    public const string Section = "InstrumentsProducer";

    [Required(ErrorMessage = "InstrumentPriceChangedTopic is undefined")]
    public string InstrumentPriceChangedTopic { get; set; }

    [Required(ErrorMessage = "BondCouponChangedTopic is undefined")]
    public string BondCouponChangedTopic { get; set; }

    [Required(ErrorMessage = "StockDividendChangedTopic is undefined")]
    public string StockDividendChangedTopic { get; set; }
}