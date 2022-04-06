using System;

using FluentAssertions;

using Framework.DomainDriven;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleSystem.Domain;
using SampleSystem.IntegrationTests.__Support.TestData;
using SampleSystem.WebApiCore.Controllers.Main;

namespace SampleSystem.IntegrationTests
{
    [TestClass]
    public class SqlParserTests : TestBase
    {
        [TestMethod]
        public void SaveNullIntoNotNullProperty_RequiredConstraintDALException()
        {
            // Arrange
            var testObject = new SqlParserTestObj();

            // Act
            Action action = () => this.DataHelper.EvaluateWrite(
                                                                context => { context.Logics.SqlParserTestObj.Save(testObject); });

            // Assert
            action.Should().Throw<RequiredConstraintDALException>()
                  .And.Message.Should()
                  .Contain("The field \'notNullColumn\' of type SqlParserTestObj must be initialized");
        }

        [TestMethod]
        public void SaveNotUniqueProperty_UniqueViolationConstraintDALException()
        {
            // Arrange
            var testObject1 = new SqlParserTestObj { UniqueColumn = "1", NotNullColumn = "2" };
            var testObject2 = new SqlParserTestObj { UniqueColumn = "1", NotNullColumn = "2" };

            // Act
            Action action = () => this.DataHelper.EvaluateWrite(
                                                                context =>
                                                                    {
                                                                        context.Logics.SqlParserTestObj.Save(testObject1);
                                                                        context.Logics.SqlParserTestObj.Save(testObject2);
                                                                    });

            // Assert
            action.Should().Throw<UniqueViolationConstraintDALException>()
                  .And.Message.Should()
                  .Contain("SqlParserTestObj with same:\'UniqueColumn\' already exists");
        }

        [TestMethod]
        public void RemoveLinkedObject_RemoveLinkedObjectsDALException()
        {
            // Arrange
            var testObject = new SqlParserTestObj { UniqueColumn = "1", NotNullColumn = "2" };
            var testObjectContainer = new SqlParserTestObjContainer { IncludedObject = testObject };

            this.DataHelper.EvaluateWrite(
                                          context =>
                                          {
                                              context.Logics.SqlParserTestObj.Save(testObject);
                                              context.Logics.SqlParserTestObjContainer.Save(testObjectContainer);
                                          });

            // Act
            Action action = () => this.DataHelper.EvaluateWrite(
                                                                context =>
                                                                {
                                                                    context.Logics.SqlParserTestObj.Remove(testObject);
                                                                });

            // Assert
            action.Should().Throw<RemoveLinkedObjectsDALException>().And.Message.Should().Contain($"{nameof(SqlParserTestObj)} cannot be removed because it is used in {nameof(SqlParserTestObjContainer)}");
        }

        [TestMethod]
        public void RemoveHRDepartment_HasEmployeeWithHRDepartment_CorrectExceptionMessage()
        {
            // Arrange
            var employeeController = this.MainWebApi.Employee;
            var hRDepartmentController = this.GetController<HRDepartmentController>();

            var buTypeId = this.DataHelper.SaveBusinessUnitType(DefaultConstants.BUSINESS_UNIT_TYPE_COMPANY_ID);

            var luxoftBuId = this.DataHelper.SaveBusinessUnit(
                id: DefaultConstants.BUSINESS_UNIT_PARENT_COMPANY_ID,
                name: DefaultConstants.BUSINESS_UNIT_PARENT_COMPANY_NAME,
                type: buTypeId);

            var costBuId = this.DataHelper.SaveBusinessUnit(
                id: DefaultConstants.BUSINESS_UNIT_PARENT_CC_ID,
                name: DefaultConstants.BUSINESS_UNIT_PARENT_CC_NAME,
                type: buTypeId,
                parent: luxoftBuId);

            var location = this.DataHelper.SaveLocation(name: "testLocation");

            var employeeIdentity = this.DataHelper.SaveEmployee(login: "value", coreBusinessUnit: costBuId, location: location);

            var fullEmployee = employeeController.GetFullEmployee(employeeIdentity);

            // Act
            Action action = () => hRDepartmentController.RemoveHRDepartment(fullEmployee.HRDepartment.Identity);

            // Assert
            action.Should().Throw<Exception>().WithMessage($"{nameof(HRDepartment)} cannot be removed because it is used in {nameof(Employee)}");
        }
    }
}
