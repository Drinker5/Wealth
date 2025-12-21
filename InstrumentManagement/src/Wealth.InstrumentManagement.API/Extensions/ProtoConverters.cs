using Wealth.BuildingBlocks;
using Wealth.InstrumentManagement.Application.Instruments.Models;

namespace Wealth.InstrumentManagement.API.Extensions;

public static class ProtoConverters
{
    public static InstrumentProto ToProto(this Instrument instrument) =>
        new()
        {
            Id = instrument.Id,
            InstrumentId = instrument.InstrumentId,
            Type = instrument.Type.ToProto(),
        };
}