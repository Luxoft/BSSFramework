using SampleSystem.IntegrationTests.__Support.TestData;
using SampleSystem.WebApiCore.Controllers.Main;

namespace SampleSystem.IntegrationTests;

public class ImpersonateTests(IServiceProvider rootServiceProvider) : TestBase(rootServiceProvider)
{
    [Fact]
    public async Task SaveDataWithImpersonate_ImpersonateWork()
    {
        // Arrange
        var testImpersonateLogin = Guid.NewGuid().ToString();

        var controllerEvaluator = this.GetControllerEvaluator<ImpersonateController>();

        // Act
        var testObj = await controllerEvaluator.EvaluateAsync(c => c.TestSave(testImpersonateLogin, default));

        var fullList = await controllerEvaluator.EvaluateAsync(c => c.GetFullList(default));

        // Assert
        var savedObject = Assert.Single(fullList, obj => obj.Id == testObj.Id);
        Assert.Equal(testImpersonateLogin, savedObject.ModifiedBy);
    }

}
