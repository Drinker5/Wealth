using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement.Application.Instruments.Models;

namespace Wealth.InstrumentManagement.Application.Repositories;

public interface IInstrumentsRepository
{
    Task<IReadOnlyCollection<Instrument>> GetInstruments(
        IReadOnlyCollection<InstrumentId> instrumentIds, 
        CancellationToken token);
}