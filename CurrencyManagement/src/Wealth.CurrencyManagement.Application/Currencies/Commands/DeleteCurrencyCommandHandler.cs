using Wealth.CurrencyManagement.Application.Abstractions;
using Wealth.CurrencyManagement.Domain.Repositories;

namespace Wealth.CurrencyManagement.Application.Currencies.Commands;

public class DeleteCurrencyCommandHandler : ICommandHandler<DeleteCurrencyCommand>
{
    private readonly ICurrencyRepository currencyRepository;

    public DeleteCurrencyCommandHandler(ICurrencyRepository currencyRepository)
    {
        this.currencyRepository = currencyRepository;
    }
    
    public Task Handle(DeleteCurrencyCommand request, CancellationToken cancellationToken)
    {
        return currencyRepository.DeleteCurrency(request.CurrencyId);
    }
}