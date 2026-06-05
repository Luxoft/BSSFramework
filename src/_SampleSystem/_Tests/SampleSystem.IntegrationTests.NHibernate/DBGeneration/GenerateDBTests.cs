using Framework.AutomationCore.Extensions;

using SampleSystem.DbGenerate.NHibernate;
using SampleSystem.IntegrationTests._Environment;
using SampleSystem.IntegrationTests._Environment.TestData;

namespace SampleSystem.IntegrationTests.DBGeneration;

public class GenerateDBTests(IServiceProvider rootServiceProvider) : TestBase(rootServiceProvider)
{
    [Fact]
    public void GenerateDB_SecondTime_ShouldNotFail()
    {
        // Arrange
        var generator = new DbGeneratorTest();

        // Act
        var ex = Record.Exception(() => generator.GenerateAllDB(
                                      this.ActualConnectionString.DataSource,
                                      this.ActualConnectionString.InitialCatalog,
                                      credential: this.ActualConnectionString.TryGetDbUserCredential()));

        // Assert
        Assert.Null(ex);
    }
}

