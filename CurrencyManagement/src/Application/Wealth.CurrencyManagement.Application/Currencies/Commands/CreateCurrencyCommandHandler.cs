using Wealth.CurrencyManagement.Application.Abstractions;
using Wealth.CurrencyManagement.Application.Currencies.Queries;
using Wealth.CurrencyManagement.Domain.Repositories;

namespace Wealth.CurrencyManagement.Application.Currencies.Commands;

public class CreateCurrencyCommandHandler(ICurrencyRepository repository) : ICommandHandler<CreateCurrencyCommand, CurrencyDTO>
{
    public async Task<CurrencyDTO> Handle(CreateCurrencyCommand command, CancellationToken cancellationToken)
    {
        var currency = await repository.CreateCurrency(command.CurrencyId, command.Name, command.Symbol);
        return CurrencyDTO.From(currency);
    }
}