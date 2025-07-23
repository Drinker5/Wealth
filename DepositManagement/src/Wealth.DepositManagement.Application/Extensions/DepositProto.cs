using Wealth.DepositManagement.Domain.Deposits;

namespace Wealth.DepositManagement;

public partial class DepositProto
{
    public static implicit operator Deposit(DepositProto grpcValue)
    {
        throw new NotSupportedException();
    }

    public static implicit operator DepositProto(Deposit value)
    {
        return new DepositProto
        {
            DepositId = value.Id,
            Name = value.Name,
            Investment = value.Investment,
            Yield = value.Yield,
            InterestPerYear = value.InterestPerYear,
        };
    }
}