namespace Wealth.InstrumentManagement.Application.Services;

public interface IPriceUpdater
{
    Task UpdatePrices(CancellationToken token);
}