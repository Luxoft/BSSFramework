using System;

using FluentAssertions;

using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.Validation;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using WorkflowSampleSystem.Domain;
using WorkflowSampleSystem.Generated.DTO;
using WorkflowSampleSystem.IntegrationTests.__Support.TestData;
using WorkflowSampleSystem.WebApiCore.Controllers.Main;

namespace WorkflowSampleSystem.IntegrationTests
{
    [TestClass]
    public class FixedPropertyValidatorTests : TestBase
    {
        [TestMethod]
        public void PrimitiveImmutablePropertyChanged_RaisedValidationError()
        {
            // Arrange
            var testImmutableObjController = this.GetController<TestImmutableObjController>();
            var identity = testImmutableObjController.SaveTestImmutableObj(
                new TestImmutableObjStrictDTO { TestImmutablePrimitiveProperty = "AAA" });

            // Act
            Action changePropertyAction = () =>
                                          {
                                              var dto = testImmutableObjController.GetRichTestImmutableObj(identity);
                                              dto.TestImmutablePrimitiveProperty = "BBB";
                                              testImmutableObjController.SaveTestImmutableObj(dto.ToStrict());
                                          };

            // Assert
            changePropertyAction.Should()
                                .Throw<Exception>(
                                    $"{nameof(TestImmutableObj.TestImmutablePrimitiveProperty)} field in {nameof(TestImmutableObj)} can't be changed");
        }

        [TestMethod]
        public void ReferenceImmutablePropertyChanged_RaisedValidationError()
        {
            // Arrange
            var employeeController = this.GetController<WorkflowSampleSystem.WebApiCore.Controllers.Main.EmployeeController>();
            var testImmutableObjController = this.GetController<WorkflowSampleSystem.WebApiCore.Controllers.Main.TestImmutableObjController>();
            var identity = testImmutableObjController.SaveTestImmutableObj(new TestImmutableObjStrictDTO { });

            // Act
            Action changePropertyAction = () =>
                                          {
                                              var dto = testImmutableObjController.GetRichTestImmutableObj(identity);
                                              dto.TestImmutableRefProperty = employeeController.GetSimpleEmployee(
                                                  this.DataHelper.GetEmployeeByLogin(
                                                      this.AuthHelper.GetCurrentUserLogin()));
                                              testImmutableObjController.SaveTestImmutableObj(dto.ToStrict());
                                          };

            // Assert
            changePropertyAction.Should()
                                .Throw<Exception>(
                                    $"{nameof(TestImmutableObj.TestImmutableRefProperty)} field in {nameof(TestImmutableObj)} can't be changed");
        }

        [TestMethod]
        public void ImmutablePropertyInitializedByIntegration_ShouldNotThrowException()
        {
            // Arrange
            var integrationController = this.GetController<WorkflowSampleSystem.WebApiCore.Controllers.Integration.TestImmutableObjController>();

            // Act
            Action insertAction = () => integrationController.SaveTestImmutableObj(new TestImmutableObjIntegrationRichDTO { TestImmutablePrimitiveProperty = "AAA", Id = Guid.NewGuid() });

            // Assert
            insertAction.Should().NotThrow();
        }

        [TestMethod]
        public void ImmutablePropertyChangedByIntegration_RaisedValidationError()
        {
            // Arrange
            var integrationTestImmutableObjController = this.GetController<WorkflowSampleSystem.WebApiCore.Controllers.Integration.TestImmutableObjController>();

            var identity = integrationTestImmutableObjController.SaveTestImmutableObj(new TestImmutableObjIntegrationRichDTO { TestImmutablePrimitiveProperty = "AAA", Id = Guid.NewGuid() });

            // Act
            Action changePropertyAction = () => integrationTestImmutableObjController.SaveTestImmutableObj(new TestImmutableObjIntegrationRichDTO { TestImmutablePrimitiveProperty = "BBB", Id = identity.Id });

            // Assert
            changePropertyAction.Should().Throw<Exception>($"{nameof(TestImmutableObj.TestImmutablePrimitiveProperty)} field in {nameof(TestImmutableObj)} can't be changed");
        }

    }
}
