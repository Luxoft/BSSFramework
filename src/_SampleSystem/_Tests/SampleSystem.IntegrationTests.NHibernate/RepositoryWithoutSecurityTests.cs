using Anch.Testing.Xunit;

using SampleSystem.IntegrationTests._Environment.TestData;
using SampleSystem.WebApiCore.Controllers.Main;

namespace SampleSystem.IntegrationTests;

public class RepositoryWithoutSecurityTests(IServiceProvider rootServiceProvider) : TestBase(rootServiceProvider)
{
    [AnchFact]
    public async Task GetDataFromUnsecurityRepository_DataLoaded(CancellationToken ct)
    {
        // Arrange
        var controllerEvaluator = this.GetControllerEvaluator<NoSecurityController>();

        // Act
        var testObj = await controllerEvaluator.EvaluateAsync(c => c.TestSave(ct));

        var fullList = await controllerEvaluator.EvaluateAsync(c => c.GetFullList(ct));

        // Assert
        Assert.Contains(testObj, fullList);
    }

    [AnchFact]
    public async Task GetDataFromUnsecurityRepository_TryLoadWithSecurity_DataLoadFaileds(CancellationToken ct)
    {
        // Arrange
        var controllerEvaluator = this.GetControllerEvaluator<NoSecurityController>();

        // Act
        var ex = await Record.ExceptionAsync(() => controllerEvaluator.EvaluateAsync(c => c.TestFaultSave(ct)));

        // Assert
        Assert.IsType<ArgumentOutOfRangeException>(ex);
    }
}

