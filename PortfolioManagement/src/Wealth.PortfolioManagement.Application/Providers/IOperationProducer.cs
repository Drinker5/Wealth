namespace Wealth.PortfolioManagement.Application.Providers;

public interface IOperationProducer
{
    Task ProduceOperations(DateTimeOffset from, CancellationToken token);
}