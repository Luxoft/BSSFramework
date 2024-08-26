using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleSystem.Domain;
using SampleSystem.IntegrationTests.__Support.TestData;

using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.Repository;
using Framework.SecuritySystem;
using Framework.SecuritySystem.UserSource;

using Microsoft.Extensions.DependencyInjection;

namespace SampleSystem.IntegrationTests;

[TestClass]
public class SecurityRuleTests : TestBase
{
    [TestMethod]
    public void ApplyExceptRule_CurrentUserExcepted()
    {
        // Arrange
        var testSecurityRule = SecurityRole.Administrator.Except(DomainSecurityRule.CurrentUser);

        var testOtherEmployeeId = this.DataHelper.SaveEmployee().Id;

        var currentEmployeeId = this.DataHelper.GetCurrentEmployee().Id;

        var testObjectIdents = this.Evaluate(
            DBSessionMode.Write,
            ctx =>
            {
                var bll = ctx.Logics.Default.Create<TestExceptObject>();

                var employeeRep = ctx.ServiceProvider.GetRequiredKeyedService<IRepository<Employee>>(nameof(SecurityRule.Disabled));

                var testObj1 = new TestExceptObject { Employee = employeeRep.Load(testOtherEmployeeId) };
                var testObj2 = new TestExceptObject { Employee = employeeRep.Load(currentEmployeeId) };
                bll.Save([testObj1, testObj2]);

                return new[] { testObj1.Id, testObj2.Id };
            });

        // Act
        var loadedObjects = this.Evaluate(
            DBSessionMode.Read,
            ctx => ctx.Logics
                      .Default
                      .Create<TestExceptObject>(testSecurityRule)
                      .GetSecureQueryable()
                      .Select(obj => obj.Id).ToList());

        // Assert
        loadedObjects.Should().BeEquivalentTo(new[] { testObjectIdents[0] });
    }

    [TestMethod]
    public void ApplyOverrideFaultMessageRule_FaultMessageChanged()
    {
        // Arrange
        var faultMessage = "TestFaultMessage";

        var testSecurityRule = DomainSecurityRule.AccessDenied.WithOverrideAccessDeniedMessage(faultMessage);

        // Act
        var action = () => this.Evaluate(
                         DBSessionMode.Read,
                         ctx =>
                         {
                             var bll = ctx.Logics.EmployeeFactory.Create(testSecurityRule);

                             bll.CheckAccess(ctx.Logics.Employee.GetCurrent());
                         });

        // Assert
        action.Should()
              .Throw<Exception>()
              .WithMessage(faultMessage);
    }
}

[TestClass]
public class CurrentUserSecurityProvideTests : TestBase
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
