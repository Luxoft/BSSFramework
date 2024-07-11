using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleSystem.Domain;
using SampleSystem.IntegrationTests.__Support.TestData;

using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.Repository;
using Framework.SecuritySystem;

using Microsoft.Extensions.DependencyInjection;

namespace SampleSystem.IntegrationTests;

[TestClass]
public class ExceptSecurityTests : TestBase
{
    [TestMethod]
    public void TryApplyExceptRule_CurrentUserExcepted()
    {
        // Arrange
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
                      .Create<TestExceptObject>(SecurityRole.Administrator.Except(SecurityRule.CurrentUser))
                      .GetSecureQueryable()
                      .Select(obj => obj.Id).ToList());

        // Assert
        loadedObjects.Should().BeEquivalentTo(new[] { testObjectIdents[0] });
    }
}
