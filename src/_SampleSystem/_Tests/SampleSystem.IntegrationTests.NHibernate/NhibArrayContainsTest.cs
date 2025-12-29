using Framework.DomainDriven;

using SampleSystem.IntegrationTests.__Support.TestData;

namespace SampleSystem.IntegrationTests;

[TestClass]
public class NhibArrayContainsTest : TestBase
{

    [TestMethod]
    public void LinqContainsOverArray_TranslatedAndExecutedCorrectly()
    {
        // Arrange
        var bu = this.DataHelper.SaveBusinessUnit();

        // Act
        var buId = this.Evaluate(DBSessionMode.Read,
                                   ctx =>
                                   {
                                       var arr = new[] { bu.Id };

                                       return ctx.Logics.BusinessUnit.GetUnsecureQueryable().Where(v => arr.Contains(v.Id)).Select(v => v.Id).Single();
                                   });

        // Assert
        buId.Should().Be(bu.Id);
    }
}
