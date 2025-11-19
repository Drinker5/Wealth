using Wealth.BuildingBlocks.Application;
using Wealth.PortfolioManagement.Domain.Repositories;

namespace Wealth.PortfolioManagement.Application.Portfolios.Commands;

public class AddCurrencyHandler(IPortfolioRepository repository) : ICommandHandler<AddCurrency>
{
    public Task Handle(AddCurrency request, CancellationToken cancellationToken)
    {
        return repository.AddCurrency(request.PortfolioId, request.Money.Currency, request.Money.Amount);
    }
}