using Wealth.InstrumentManagement.Application.Instruments.Models;

namespace Wealth.InstrumentManagement.Application.Repositories;

public interface IInstrumentsRepository
{
    Task<IReadOnlyCollection<Instrument>> GetInstruments(
        IReadOnlyCollection<string> requestIsins, 
        CancellationToken token);
}