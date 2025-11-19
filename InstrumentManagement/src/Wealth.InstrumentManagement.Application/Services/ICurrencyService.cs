using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.InstrumentManagement.Application.Services;

public interface ICurrencyService
{
    Task<bool> IsCurrencyExists(CurrencyCode currency);
}