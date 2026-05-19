using SampleSystem.DbGenerate.NHibernate;
using SampleSystem.IntegrationTests._Environment.TestData;

namespace SampleSystem.IntegrationTests.DBGeneration;

public class SchemeUpdateTests(IServiceProvider rootServiceProvider) : TestBase(rootServiceProvider)
{
    [Fact]
    public void SchemeUpdate_ExistsDatabase_ShouldNotFail()
    {
        // Arrange

        // Act
        var ex = Record.Exception(() => UseSchemeUpdateTest.UseSchemeUpdate(this.DatabaseContext.ConnectionString.Value));

        // Assert
        Assert.Null(ex);
    }
}
