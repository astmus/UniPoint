using NUnit.Framework;

using static MissBot.Application.IntegrationTests.Testing;

namespace MissBot.Application.IntegrationTests;
[TestFixture]
public abstract class BaseTestFixture
{
    [SetUp]
    public async Task TestSetUp()
    {
        await ResetState();
    }
}
