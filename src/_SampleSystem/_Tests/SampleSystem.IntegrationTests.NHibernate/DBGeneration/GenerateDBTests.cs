using Framework.Database.NHibernate.DBGenerator;

using SampleSystem.DbGenerate.NHibernate;
using SampleSystem.IntegrationTests.__Support.TestData;

namespace SampleSystem.IntegrationTests.DBGeneration;

public class GenerateDBTests(IServiceProvider rootServiceProvider) : TestBase(rootServiceProvider)
{
    [Fact]
    public void GenerateDB_SecondTime_ShouldNotFail()
    {
        // Arrange
        var generator = new DbGeneratorTest();

        // Act
        var action = new Action(() => generator.GenerateAllDB(
                                                              this.DatabaseContext.Main.DataSource,
                                                              this.DatabaseContext.Main.DatabaseName,
                                                              credential: DbUserCredential.Create(
                                                               this.DatabaseContext.Main.UserId,
                                                               this.DatabaseContext.Main.Password)));

        // Assert
        Assert.Null(Record.Exception(action));
    }
}
