using Anch.Testing.Xunit;

using Microsoft.Extensions.DependencyInjection;

using SampleSystem.IntegrationTests._Environment;
using SampleSystem.IntegrationTests._Environment.TestData;

namespace SampleSystem.IntegrationTests.DBGeneration;

public class GenerateDBTests(IServiceProvider rootServiceProvider) : TestBase(rootServiceProvider)
{
    [AnchFact]
    public async Task GenerateDB_SecondTime_ShouldNotFail(CancellationToken ct)
    {
        // Arrange
        var initializer = rootServiceProvider.GetRequiredService<EmptySchemaInitializer>();

        // Act
        var ex = await Record.ExceptionAsync(() => initializer.Initialize(ct));

        // Assert
        Assert.Null(ex);
    }
}
