using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.SecuritySystem;
using Framework.SecuritySystem.UserSource;

using Microsoft.Extensions.DependencyInjection;

using SampleSystem.Domain;
using SampleSystem.IntegrationTests.__Support.TestData;

namespace SampleSystem.IntegrationTests;

[TestClass]
public class CurrentUserSecurityProviderTests : TestBase
{
    private Guid[] testObjectIdents;

    [TestInitialize]
    public void Setup()
    {
        this.testObjectIdents = this.Evaluate(
            DBSessionMode.Write,
            ctx =>
            {
                var bll = ctx.Logics.Default.Create<TestRelativeEmployeeObject>();

                var currentEmployee = ctx.ServiceProvider.GetRequiredService<ICurrentUserSource<Employee>>().CurrentUser;

                var testObj1 = new TestRelativeEmployeeObject { EmployeeRef1 = currentEmployee };
                var testObj2 = new TestRelativeEmployeeObject { EmployeeRef2 = currentEmployee };
                bll.Save([testObj1, testObj2]);

                return new[] { testObj1.Id, testObj2.Id };
            });
    }

    [TestMethod]
    [DynamicData(nameof(TestRelativeEmployeeObject_FilterByPrimaryEmployeeRef_EmployeeBySecondaryRefMissed_Source), DynamicDataSourceType.Method)]
    public void TestRelativeEmployeeObject_FilterByEmployeeRef1_EmployeeRef2Missed(string propName, int expectedIndex)
    {
        // Arrange
        var testSecurityRule = new DomainSecurityRule.CurrentUserSecurityRule(propName);

        // Act
        var loadedObjects = this.Evaluate(
            DBSessionMode.Read,
            ctx => ctx.Logics
                      .Default
                      .Create<TestRelativeEmployeeObject>(testSecurityRule)
                      .GetSecureQueryable()
                      .Select(obj => obj.Id).ToList());

        // Assert
        loadedObjects.Should().BeEquivalentTo(new[] { this.testObjectIdents[expectedIndex] });
    }

    private static IEnumerable<object[]> TestRelativeEmployeeObject_FilterByPrimaryEmployeeRef_EmployeeBySecondaryRefMissed_Source()
    {
        return
        [
            [nameof(TestRelativeEmployeeObject.EmployeeRef1), 0],
            [nameof(TestRelativeEmployeeObject.EmployeeRef2), 1],
        ];
    }
}
