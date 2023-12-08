using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleSystem.IntegrationTests.__Support.TestData;
using SampleSystem.WebApiCore.Controllers.Main;

namespace SampleSystem.IntegrationTests;

[TestClass]
public class ImpersonateTests : TestBase
{
    [TestMethod]
    public async Task SaveDataWithImpersonate_ImpersonateWork()
    {
        // Arrange
        var testImpersonateLogin = Guid.NewGuid().ToString();

        var controllerEvaluator = this.GetControllerEvaluator<ImpersonateController>();

        // Act
        var testObj = await controllerEvaluator.EvaluateAsync(c => c.TestSave(testImpersonateLogin, default));

        var fullList = await controllerEvaluator.EvaluateAsync(c => c.GetFullList(default));

        // Assert
        fullList.Should().ContainSingle(obj => obj.Id == testObj.Id)
                .Subject.ModifiedBy.Should().Be(testImpersonateLogin);
    }

}
