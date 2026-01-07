using System.ComponentModel.DataAnnotations;

namespace Wealth.PortfolioManagement.Application.Options;

public sealed class OperationsProducerOptions
{
    public const string Section = "OperationsProducer";

    [Required(ErrorMessage = "OperationsProducer's topic is undefined")]
    public string Topic { get; set; }
}