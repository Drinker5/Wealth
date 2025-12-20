using AutoFixture;

namespace Wealth.InstrumentManagement.Application.Tests;

[SetUpFixture]
public class GlobalSetup
{
    public static readonly IFixture Fixture = new Fixture();
    
    [OneTimeSetUp]
    public void RunBeforeAnyTests()
    {
        Fixture.Customizations.Add(new CustomBuilder());
    }
}