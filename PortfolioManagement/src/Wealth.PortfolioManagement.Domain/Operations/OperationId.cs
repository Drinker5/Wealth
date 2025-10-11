namespace Wealth.PortfolioManagement.Domain.Operations;

public readonly record struct OperationId(string Value)
{
    public static OperationId NewId() => new(Guid.NewGuid().ToString("N"));
    
    public static implicit operator OperationId(string value) => new(value);
}