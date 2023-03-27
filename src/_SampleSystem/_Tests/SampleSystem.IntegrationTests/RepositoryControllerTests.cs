using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleSystem.IntegrationTests.__Support.TestData;
using SampleSystem.WebApiCore.Controllers.Main;

namespace SampleSystem.IntegrationTests;

[TestClass]
public class RepositoryControllerTests : TestBase
{
    [TestMethod]
    public async Task TestLoadFromRepository_CurrentEmployeeLoaded()
    {
        // Arrange
        var repositoryController = this.GetControllerEvaluator<TestRepositoryController>();

        var employeeController = this.GetControllerEvaluator<EmployeeAsyncController>();

        // Act
        var result = await repositoryController.EvaluateAsync(c => c.LoadPair(default));

        var currentEmployee = await employeeController.EvaluateAsync(c => c.GetCurrentEmployee(default));

        // Assert
        result.Employees.Should().Contain(currentEmployee.Identity);
    }
}
