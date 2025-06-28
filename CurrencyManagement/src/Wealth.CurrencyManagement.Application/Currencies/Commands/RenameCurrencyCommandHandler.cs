using Wealth.BuildingBlocks.Application;
using Wealth.CurrencyManagement.Domain.Repositories;

namespace Wealth.CurrencyManagement.Application.Currencies.Commands;

public class RenameCurrencyCommandHandler(ICurrencyRepository repository) : ICommandHandler<RenameCurrencyCommand>
{
    public Task Handle(RenameCurrencyCommand command, CancellationToken cancellationToken)
    {
        return repository.ChangeCurrencyName(command.CurrencyId, command.NewName);
    }
}