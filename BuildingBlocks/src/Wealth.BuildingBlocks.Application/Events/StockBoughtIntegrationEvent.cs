using System.Text.Json.Serialization;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.BuildingBlocks.Application.Events;

public record StockBoughtIntegrationEvent : IntegrationEvent
{
    public PortfolioId PortfolioId { get; set; }
    public InstrumentId InstrumentId { get; set; }
    public Money TotalPrice { get; set; }
    public int Quantity { get; set; }

    [JsonIgnore]
    public override string Key => PortfolioId.ToString();
}