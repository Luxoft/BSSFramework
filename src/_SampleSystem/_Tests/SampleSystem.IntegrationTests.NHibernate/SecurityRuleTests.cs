using Framework.Application;
using Framework.Application.Repository;
using Framework.BLL;
using Framework.Database;

using SecuritySystem;

using Microsoft.Extensions.DependencyInjection;

using SampleSystem.Domain.Employee;
using SampleSystem.IntegrationTests.__Support.TestData;

using SecuritySystem.AccessDenied;

namespace SampleSystem.IntegrationTests;

public class SecurityRuleTests : TestBase
{
    [Fact]
    public async Task ApplyExceptRule_CurrentUserExcepted()
    {
        // Arrange
        var testSecurityRule = SecurityRole.Administrator.Except(DomainSecurityRule.CurrentUser);

        var testOtherEmployeeId = this.DataHelper.SaveEmployee().Id;

        var currentEmployeeId = this.DataHelper.GetCurrentEmployee().Id;

        var testObjectIdents = await this.EvaluateAsync(
            DBSessionMode.Write,
            async ctx =>
            {
                var bll = ctx.Logics.Default.Create<TestExceptObject>();

                var employeeRep = ctx.ServiceProvider.GetRequiredKeyedService<IRepository<Employee>>(nameof(SecurityRule.Disabled));

                var testObj1 = new TestExceptObject { Employee = await employeeRep.LoadAsync(testOtherEmployeeId) };
                var testObj2 = new TestExceptObject { Employee = await employeeRep.LoadAsync(currentEmployeeId) };
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
        Assert.Equal(new[] { testObjectIdents[0] }, loadedObjects);
    }

    [Fact]
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

                             bll.CheckAccess(ctx.CurrentEmployeeSource.CurrentUser);
                         });

        // Assert
        Assert.Equal(faultMessage, Assert.Throws<AccessDeniedException>(action).Message);
    }
}
