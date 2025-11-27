using Wealth.Aggregation.Application.Repository;
using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.PortfolioManagement;

namespace Wealth.Aggregation.Application.Commands;

public record MoneyOperation(
    string Id,
    DateTime Date,
    PortfolioId PortfolioId,
    Money Amount,
    MoneyOperationTypeProto Type) : ICommand;
    
public class MoneyOperationHandler(IMoneyOperationRepository repository) : ICommandHandler<MoneyOperation>
{
    public Task Handle(MoneyOperation request, CancellationToken cancellationToken)
    {
        return repository.Upsert(request, cancellationToken);
    }
}
