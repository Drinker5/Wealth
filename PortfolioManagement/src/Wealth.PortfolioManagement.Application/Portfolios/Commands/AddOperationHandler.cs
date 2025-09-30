using Wealth.BuildingBlocks.Application;
using Wealth.PortfolioManagement.Domain.Repositories;

namespace Wealth.PortfolioManagement.Application.Portfolios.Commands;

public class AddOperationHandler(IOperationRepository repository) : ICommandHandler<AddOperation>
{
    public Task Handle(AddOperation request, CancellationToken cancellationToken)
    {
        return repository.CreateOperation(request.Operation, cancellationToken);
    }
}