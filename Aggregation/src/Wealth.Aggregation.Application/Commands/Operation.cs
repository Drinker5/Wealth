using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.Aggregation.Application.Commands;

public sealed record Operation(
    string Id,
    DateTime Date,
    PortfolioId PortfolioId,
    int InstrumentId,
    InstrumentType instrumentType,
    Money Amount,
    OperationType Type,
    long Quantity = -1) : ICommand;

public enum OperationType : byte
{
    None = 0,
    Buy = 1,
    Sell = 2,
    Coupon = 3,
    CouponTax = 4,
    Amortization = 5,
    Dividend = 6,
    DividendTax = 7,
    BrokerFee = 8,
    Deposit = 9,
    Withdraw = 10,
}