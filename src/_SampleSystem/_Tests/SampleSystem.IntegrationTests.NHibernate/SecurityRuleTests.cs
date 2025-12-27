using SampleSystem.Domain;

using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.Repository;
using SecuritySystem;

using Microsoft.Extensions.DependencyInjection;

using SampleSystem.IntegrationTests.__Support.TestData;

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
