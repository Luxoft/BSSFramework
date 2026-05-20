using Anch.Testing.Xunit;

using Framework.AutomationCore.Extensions;
using Framework.Database.NHibernate.DBGenerator;


using SampleSystem.DbGenerate.NHibernate;
using SampleSystem.IntegrationTests._Environment.TestData;

namespace SampleSystem.IntegrationTests.DBGeneration;

public class GenerateDBTests(IServiceProvider rootServiceProvider) : TestBase(rootServiceProvider)
{
    [AnchFact]
    public async Task GenerateDB_SecondTime_ShouldNotFail(CancellationToken ct)
    {
        // Arrange
        var generator = new DbGeneratorTest();

        // Act
        var ex = Record.Exception(() => generator.GenerateAllDB(
                                      this.ActualConnectionString.DataSource,
                                      this.ActualConnectionString.InitialCatalog,
                                      credential: DbUserCredential.Create(
                                          this.ActualConnectionString.UserId,
                                          this.ActualConnectionString.Password)));

        // Assert
        Assert.Null(ex);
    }
}
