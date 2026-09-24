using Anch.Testing.Xunit;

using SampleSystem.IntegrationTests._Environment.TestData;

namespace SampleSystem.IntegrationTests;

public class EmptyTests(IServiceProvider rootServiceProvider) : TestBase(rootServiceProvider)
{
    [AnchFact]
    public async Task InitTest(CancellationToken ct)
    {
        //var initializer = this.RootServiceProvider.GetRequiredKeyedService<IInitializer>(TestDatabaseInitializer.TestDataKey);

        //await initializer.Initialize(ct);
    }
}
