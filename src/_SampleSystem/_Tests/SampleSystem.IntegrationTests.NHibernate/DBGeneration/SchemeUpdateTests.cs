using SampleSystem.IntegrationTests.__Support.TestData;
using SampleSystem.DbGenerate;

namespace SampleSystem.IntegrationTests.DBGeneration;

[TestClass]
public class SchemeUpdateTests : TestBase
{
    [TestMethod]
    public void SchemeUpdate_ExistsDatabase_ShouldNotFail()
    {
        // Arrange
        var generator = new UseSchemeUpdateTest();

        // Act
        var action = new Action(() => UseSchemeUpdateTest.UseSchemeUpdate(this.DatabaseContext.Main.ConnectionString));

        // Assert
        action.Should().NotThrow();
    }
}
