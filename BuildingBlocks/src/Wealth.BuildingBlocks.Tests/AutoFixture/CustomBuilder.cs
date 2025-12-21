using AutoFixture.Kernel;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.BuildingBlocks.Tests.AutoFixture;

public class CustomBuilder : ISpecimenBuilder
{
    private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    private readonly Random random = new Random();

    public object Create(object request, ISpecimenContext context)
    {
        if (request is Type type)
        {
            if (type == typeof(ISIN))
            {
                var isinValue = new string(Enumerable.Repeat(chars, 12)
                    .Select(s => s[random.Next(s.Length)]).ToArray());
                return new ISIN(isinValue);
            }

            if (type == typeof(FIGI))
            {
                var figiValue = new string(Enumerable.Repeat(chars, 12)
                    .Select(s => s[random.Next(s.Length)]).ToArray());
                return new FIGI(figiValue);
            }

            if (type == typeof(Ticker))
            {
                var tickerValue = new string(Enumerable.Repeat(chars, Ticker.MaxLength)
                    .Select(s => s[random.Next(s.Length)]).ToArray());
                return new Ticker(tickerValue);
            }
        }

        return new NoSpecimen();
    }
}