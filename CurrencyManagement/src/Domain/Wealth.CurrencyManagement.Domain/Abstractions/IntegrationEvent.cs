using System.Text.Json.Serialization;

namespace Wealth.CurrencyManagement.Domain.Abstractions;

public abstract record IntegrationEvent
{
    [JsonInclude]
    public Guid Id { get; set; } = Guid.NewGuid();

    [JsonInclude]
    public DateTime CreationDate { get; set; } = DateTime.UtcNow;
}