using SampleSystem.IntegrationTests._Environment.TestData;

namespace SampleSystem.IntegrationTests.WebApi;

public class WebApiTests(IServiceProvider rootServiceProvider) : TestBase(rootServiceProvider)
{
    [Fact]
    public void WebApi_CallMethod()
    {
        // Arrange
        var employeeIdentity = this.DataManager.SaveEmployee(Guid.NewGuid());

        var employeeController = this.MainWebApi.Employee;

        // Act
        var employees = employeeController.Evaluate(c => c.GetSimpleEmployees());

        // Assert
        Assert.Contains(employees, e => e.Id == employeeIdentity.Id);
    }
}
