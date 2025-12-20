using AutoFixture.Kernel;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.InstrumentManagement.Application.Tests;

public class CustomBuilder : ISpecimenBuilder
{
    private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    
    public object Create(object request, ISpecimenContext context)
    {
        if (request is Type type)
        {
            if (type == typeof(ISIN))
            {
                var random = new Random();
                var isinValue = new string(Enumerable.Repeat(chars, 12)
                    .Select(s => s[random.Next(s.Length)]).ToArray());
                return new ISIN(isinValue);
            }

            if (type == typeof(FIGI))
            {
                var random = new Random();
                var figiValue = new string(Enumerable.Repeat(chars, 12)
                    .Select(s => s[random.Next(s.Length)]).ToArray());
                return new FIGI(figiValue);
            }
        }

        return new NoSpecimen();
    }
}