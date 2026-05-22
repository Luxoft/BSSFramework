using Anch.Testing.Xunit;

using SampleSystem.IntegrationTests._Environment.TestData;
using SampleSystem.WebApiCore.Controllers.Main;

namespace SampleSystem.IntegrationTests;

public class RepositoryControllerTests(IServiceProvider rootServiceProvider) : TestBase(rootServiceProvider)
{
    [AnchFact]
    public async Task TestLoadFromRepository_CurrentEmployeeLoaded(CancellationToken ct)
    {
        // Arrange
        var repositoryController = this.GetControllerEvaluator<TestRepositoryController>();

        var employeeController = this.GetControllerEvaluator<EmployeeAsyncController>();

        // Act
        var result = await repositoryController.EvaluateAsync(c => c.LoadPair(ct));

        var currentEmployee = await employeeController.EvaluateAsync(c => c.GetCurrentEmployee(ct));

        // Assert
        Assert.Contains(currentEmployee.Identity, result.Employees);
    }
}
