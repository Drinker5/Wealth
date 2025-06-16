using Wealth.CurrencyManagement.Application.Interfaces;
using Wealth.CurrencyManagement.Domain.Repositories;

namespace Wealth.CurrencyManagement.Application.Currency.Commands;

public class CreateCurrencyCommandHandler(ICurrencyRepository repository) : ICommandHandler<CreateCurrencyCommand>
{
    public Task Handle(CreateCurrencyCommand command, CancellationToken cancellationToken)
    {
        return repository.CreateCurrency(command.CurrencyId, command.Name, command.Symbol);
    }
}