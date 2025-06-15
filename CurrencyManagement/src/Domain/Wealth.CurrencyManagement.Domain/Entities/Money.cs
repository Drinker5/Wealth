using Wealth.CurrencyManagement.Domain.Interfaces;

namespace Wealth.CurrencyManagement.Domain.Entities;

public record Money(CurrencyId CurrencyId, decimal Amount) : IValueObject;