using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleSystem.IntegrationTests.__Support.TestData;

using Framework.DomainDriven;

using SampleSystem.Generated.DTO;
using SampleSystem.WebApiCore.Controllers.Main;

namespace SampleSystem.IntegrationTests;

[TestClass]
public class RunAsTests : TestBase
{
    [TestMethod]
    public async Task SetRunAs_SystemFields_Impersonated()
    {
        // Arrange
        var testEmployeeLogin = "testEmployeeLogin";

        this.DataHelper.SaveEmployee(login: testEmployeeLogin);

        var controllerEvaluator = this.GetControllerEvaluator<TestJobObjectController>();

        await this.DataHelper.AuthHelper.SavePrincipalAsync(testEmployeeLogin);

        // Act
        await this.EvaluateAsync(DBSessionMode.Write, ctx => ctx.Authorization.RunAsManager.StartRunAsUserAsync(testEmployeeLogin));
        var objIdentity = controllerEvaluator.Evaluate(c => c.SaveTestJobObject(new TestJobObjectStrictDTO()));
        await this.EvaluateAsync(DBSessionMode.Write, ctx => ctx.Authorization.RunAsManager.FinishRunAsUserAsync());

        // Assert
        var reloadedObj = controllerEvaluator.Evaluate(c => c.GetSimpleTestJobObject(objIdentity));

        reloadedObj.CreatedBy.Should().Be(testEmployeeLogin);
        reloadedObj.ModifiedBy.Should().Be(testEmployeeLogin);
    }
}
