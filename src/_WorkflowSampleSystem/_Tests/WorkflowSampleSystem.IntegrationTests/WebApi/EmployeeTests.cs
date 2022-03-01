using System;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using WorkflowSampleSystem.IntegrationTests.__Support.TestData;
using WorkflowSampleSystem.WebApiCore.Controllers.Main;

namespace WorkflowSampleSystem.IntegrationTests
{
    [TestClass]
    public class WebApiTests : TestBase
    {
        [TestMethod]
        public void WebApi_CallMethod()
        {
            // Arrange
            var employeeIdentity = this.DataHelper.SaveEmployee(Guid.NewGuid());

            var employeeController = this.GetController<EmployeeController>(); ;

            // Act
            var employees = employeeController.GetSimpleEmployees();

            // Assert
            employees.Should().Contain(e => e.Id == employeeIdentity.Id);
        }
    }
}
