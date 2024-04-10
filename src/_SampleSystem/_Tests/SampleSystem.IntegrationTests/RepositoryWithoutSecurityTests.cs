using FluentAssertions;

using Framework.SecuritySystem;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleSystem.IntegrationTests.__Support.TestData;
using SampleSystem.WebApiCore.Controllers.Main;

namespace SampleSystem.IntegrationTests;

[TestClass]
public class RepositoryWithoutSecurityTests : TestBase
{
    [TestMethod]
    public async Task GetDataFromUnsecurityRepository_DataLoaded()
    {
        // Arrange
        var controllerEvaluator = this.GetControllerEvaluator<NoSecurityController>();

        // Act
        var testObj = await controllerEvaluator.EvaluateAsync(c => c.TestSave(default));

        var fullList = await controllerEvaluator.EvaluateAsync(c => c.GetFullList(default));

        // Assert
        fullList.Should().Contain(testObj);
    }

    [TestMethod]
    public async Task GetDataFromUnsecurityRepository_TryLoadWithSecurity_DataLoadFaileds()
    {
        // Arrange
        var controllerEvaluator = this.GetControllerEvaluator<NoSecurityController>();

        // Act
        Func<Task> saveAction = () => controllerEvaluator.EvaluateAsync(c => c.TestFaultSave(default));

        // Assert
        await saveAction.Should().ThrowAsync<InvalidOperationException>($"Security mode \"{SecurityRule.Edit}\" not allowed");
    }
}
