using Anch.Testing.Xunit;

using SampleSystem.IntegrationTests._Environment.TestData;
using SampleSystem.WebApiCore.Controllers.Main;

namespace SampleSystem.IntegrationTests;

public class ImpersonateTests(IServiceProvider rootServiceProvider) : TestBase(rootServiceProvider)
{
    [AnchFact]
    public async Task SaveDataWithImpersonate_ImpersonateWork(CancellationToken ct)
    {
        // Arrange
        var testImpersonateLogin = Guid.NewGuid().ToString();

        var controllerEvaluator = this.GetControllerEvaluator<ImpersonateController>();

        // Act
        var testObj = await controllerEvaluator.EvaluateAsync(c => c.TestSave(testImpersonateLogin, ct));

        var fullList = await controllerEvaluator.EvaluateAsync(c => c.GetFullList(ct));

        // Assert
        var savedObject = Assert.Single(fullList, obj => obj.Id == testObj.Id);
        Assert.Equal(testImpersonateLogin, savedObject.ModifiedBy);
    }

}
