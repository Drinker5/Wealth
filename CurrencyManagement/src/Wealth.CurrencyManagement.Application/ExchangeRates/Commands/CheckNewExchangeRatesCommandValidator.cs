using FluentValidation;
using Wealth.BuildingBlocks.Application.Validators;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.CurrencyManagement.Application.ExchangeRates.Commands;

public class CheckNewExchangeRatesCommandValidator : CommandValidator<CheckNewExchangeRatesCommand>
{
    public CheckNewExchangeRatesCommandValidator()
    {
        RuleFor(cmd => cmd.ToCurrency.Value)
            .Equal(CurrencyCode.RUB)
            .WithMessage("Only RUB is supported");

        RuleFor(cmd => cmd.ToCurrency.Value)
            .NotEqual(currency => currency.FromCurrency.Value)
            .WithMessage("Currencies should differ");
    }
}