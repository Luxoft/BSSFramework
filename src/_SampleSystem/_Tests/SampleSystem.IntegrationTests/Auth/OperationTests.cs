using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleSystem.IntegrationTests.__Support.TestData;
using SampleSystem.WebApiCore.Controllers.Main;

namespace SampleSystem.IntegrationTests.Auth
{
    [TestClass]
    public class OperationTests : TestBase
    {
        private const string NewDescription = "test_description";
        private const string Name = "HRDepartmentEdit";

        [TestMethod]
        public void SaveOperation_CheckOperationChanges()
        {
            // Arrange
            var employeeController = this.MainWebApi.Employee;
            var currentUser = employeeController.GetFullEmployee(
                this.DataHelper.GetEmployeeByLogin(this.AuthHelper.GetCurrentUserLogin()));

            var operationIdentity = this.GetAuthControllerEvaluator().GetSimpleOperationByName(Name).Identity;
            var operationStrict = this.GetAuthControllerEvaluator().GetFullOperation(operationIdentity).ToStrict();
            operationStrict.Description = NewDescription;

            // Act
            this.GetAuthControllerEvaluator().SaveOperation(operationStrict);

            // Assert
            var operationSimple = this.GetAuthControllerEvaluator().GetSimpleOperation(operationIdentity);

            operationSimple.Name.Should().Be(Name);
            operationSimple.Description.Should().Be(NewDescription);
            operationSimple.Active.Should().BeTrue();
            operationSimple.ModifiedBy.Should().Be(currentUser.Login.ToString());
        }
    }
}
