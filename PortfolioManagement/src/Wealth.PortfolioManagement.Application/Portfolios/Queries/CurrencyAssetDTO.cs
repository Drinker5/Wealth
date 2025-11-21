using Wealth.BuildingBlocks.Domain.Common;
using Wealth.PortfolioManagement.Domain.Portfolios;

namespace Wealth.PortfolioManagement.Application.Portfolios.Queries;

public class CurrencyAssetDTO
{
    public CurrencyId Id { get; set; }

    public int Quantity { get; set; }

    public static CurrencyAssetDTO ToDTO(CurrencyAsset currency)
    {
        return new CurrencyAssetDTO
        {
            Id = currency.CurrencyId,
            Quantity = currency.Quantity,
        };
    }
}