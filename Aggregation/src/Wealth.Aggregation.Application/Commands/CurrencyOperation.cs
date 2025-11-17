using Wealth.Aggregation.Application.Repository;
using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.PortfolioManagement;

namespace Wealth.Aggregation.Application.Commands;

public record CurrencyOperation(
    string Id,
    DateTime Date,
    PortfolioId PortfolioId,
    Money Amount,
    CurrencyOperationTypeProto Type) : ICommand;
    
public class CurrencyOperationHandler(ICurrencyOperationRepository repository) : ICommandHandler<CurrencyOperation>
{
    public Task Handle(CurrencyOperation request, CancellationToken cancellationToken)
    {
        return repository.Upsert(request, cancellationToken);
    }
}
