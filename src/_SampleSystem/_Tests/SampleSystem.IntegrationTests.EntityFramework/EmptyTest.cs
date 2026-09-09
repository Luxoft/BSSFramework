using Anch.Core;
using Anch.Testing.Database.Initializers;
using Anch.Testing.Xunit;

using Framework.Application;
using Framework.Database;

using Microsoft.Extensions.DependencyInjection;

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
