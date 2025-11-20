using FluentValidation;
using Wealth.BuildingBlocks.Application.Validators;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.CurrencyManagement.Application.ExchangeRates.Commands;

public class CheckNewExchangeRatesCommandValidator : CommandValidator<CheckNewExchangeRatesCommand>
{
    public CheckNewExchangeRatesCommandValidator()
    {
        RuleFor(cmd => cmd.ToCurrency)
            .Equal(CurrencyCode.Rub)
            .WithMessage("Only RUB is supported");

        RuleFor(cmd => cmd.ToCurrency)
            .NotEqual(currency => currency.FromCurrency)
            .WithMessage("Currencies should differ");
    }
}