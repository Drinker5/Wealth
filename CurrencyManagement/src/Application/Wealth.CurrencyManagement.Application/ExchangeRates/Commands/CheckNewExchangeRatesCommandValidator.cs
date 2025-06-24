using FluentValidation;
using Wealth.CurrencyManagement.Application.Validators;

namespace Wealth.CurrencyManagement.Application.ExchangeRates.Commands;

public class CheckNewExchangeRatesCommandValidator : CommandValidator<CheckNewExchangeRatesCommand>
{
    public CheckNewExchangeRatesCommandValidator()
    {
        RuleFor(cmd => cmd.ToCurrency.Code)
            .Equal("RUB")
            .WithMessage("Only RUB is supported");

        RuleFor(cmd => cmd.ToCurrency.Code)
            .NotEqual(currency => currency.FromCurrency.Code)
            .WithMessage("Currencies should differ");
    }
}