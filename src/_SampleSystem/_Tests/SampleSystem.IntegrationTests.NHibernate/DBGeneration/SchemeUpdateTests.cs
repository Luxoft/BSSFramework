using SampleSystem.DbGenerate.NHibernate;
using SampleSystem.IntegrationTests.__Support.TestData;

namespace SampleSystem.IntegrationTests.DBGeneration;

public class SchemeUpdateTests : TestBase
{
    [Fact]
    public void SchemeUpdate_ExistsDatabase_ShouldNotFail()
    {
        // Arrange
        var generator = new UseSchemeUpdateTest();

        // Act
        var action = new Action(() => UseSchemeUpdateTest.UseSchemeUpdate(this.DatabaseContext.Main.ConnectionString));

        // Assert
        var ex = Record.Exception(action);
        Assert.Null(ex);
    }
}
