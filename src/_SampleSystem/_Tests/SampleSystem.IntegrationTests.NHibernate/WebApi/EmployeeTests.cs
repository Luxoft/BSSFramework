﻿using SampleSystem.IntegrationTests.__Support.TestData;

namespace SampleSystem.IntegrationTests;

[TestClass]
public class WebApiTests : TestBase
{
    [TestMethod]
    public void WebApi_CallMethod()
    {
        // Arrange
        var employeeIdentity = this.DataHelper.SaveEmployee(Guid.NewGuid());

        var employeeController = this.MainWebApi.Employee;

        // Act
        var employees = employeeController.Evaluate(c => c.GetSimpleEmployees());

        // Assert
        employees.Should().Contain(e => e.Id == employeeIdentity.Id);
    }
}
