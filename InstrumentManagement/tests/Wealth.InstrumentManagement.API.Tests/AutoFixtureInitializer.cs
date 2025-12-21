using AutoFixture;
using Wealth.BuildingBlocks.Tests.AutoFixture;

namespace Wealth.InstrumentManagement.API.Tests;

public sealed class AutoFixtureInitializer
{
    public IFixture fixture;

    public AutoFixtureInitializer()
    {
        fixture = new Fixture();
        fixture.Customizations.Add(new CustomBuilder());
    }
}