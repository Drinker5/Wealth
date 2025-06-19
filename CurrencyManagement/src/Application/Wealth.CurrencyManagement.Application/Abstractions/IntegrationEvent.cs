namespace Wealth.CurrencyManagement.Application.Abstractions;

public abstract record IntegrationEvent
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public DateTime CreationDate { get; set; } = DateTime.UtcNow;
}