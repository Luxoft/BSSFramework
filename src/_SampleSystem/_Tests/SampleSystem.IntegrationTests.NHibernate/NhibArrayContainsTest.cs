using System.Linq.Expressions;

using Anch.Core;

using Framework.Application;
using Framework.AutomationCore.Utils;
using Framework.Database;

using SampleSystem.Domain.BU;
using SampleSystem.Domain.Enums;
using SampleSystem.IntegrationTests.__Support.TestData;

namespace SampleSystem.IntegrationTests;

public class NhibArrayContainsTest(IServiceProvider rootServiceProvider) : TestBase(rootServiceProvider)
{
    [Fact]
    public void LinqContainsOverArray_TranslatedAndExecutedCorrectly()
    {
        // Arrange
        var name = TextRandomizer.UniqueString("BusinessUnit");
        var bu = this.DataHelper.SaveBusinessUnit(name: name);

        var arr1 = new[] { bu.Id };
        var arr2 = new[] { BusinessUnitStatus.Current };
        var arr3 = new[] { name };

        Expression<Func<BusinessUnit, bool>> expr1 = v => arr1.Contains(v.Id);
        Expression<Func<BusinessUnit, bool>> expr2 = v => arr2.Contains(v.BusinessUnitStatus);
        Expression<Func<BusinessUnit, bool>> expr3 = v => arr3.Contains(v.Name);

        var totalFilter = new[] { expr1, expr2, expr3 }.BuildAnd();

        // Act
        var buId = this.Evaluate(
            DBSessionMode.Read,
            ctx => ctx.Logics.BusinessUnit.GetUnsecureQueryable().Where(totalFilter).Select(v => v.Id).Single());

        // Assert
        Assert.Equal(bu.Id, buId);
    }
}
