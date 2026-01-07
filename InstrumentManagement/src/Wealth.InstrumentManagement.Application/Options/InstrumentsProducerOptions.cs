using System.ComponentModel.DataAnnotations;

namespace Wealth.InstrumentManagement.Application.Options;

public sealed class InstrumentsProducerOptions
{
    public const string Section = "InstrumentsProducer";

    [Required(ErrorMessage = "InstrumentsProducer's topic is undefined")]
    public string Topic { get; set; }
}