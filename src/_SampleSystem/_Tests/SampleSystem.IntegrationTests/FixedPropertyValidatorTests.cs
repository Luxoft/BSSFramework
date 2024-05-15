using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleSystem.Domain;
using SampleSystem.Generated.DTO;
using SampleSystem.IntegrationTests.__Support.TestData;
using SampleSystem.WebApiCore.Controllers.Main;

namespace SampleSystem.IntegrationTests;

[TestClass]
public class FixedPropertyValidatorTests : TestBase
{
    [TestMethod]
    public void PrimitiveImmutablePropertyChanged_RaisedValidationError()
    {
        // Arrange
        var testImmutableObjController = this.GetControllerEvaluator<TestImmutableObjController>();
        var identity = testImmutableObjController.Evaluate(c => c.SaveTestImmutableObj(
                                                                                       new TestImmutableObjStrictDTO { TestImmutablePrimitiveProperty = "AAA" }));

        // Act
        Action changePropertyAction = () =>
                                      {
                                          var dto = testImmutableObjController.Evaluate(c => c.GetRichTestImmutableObj(identity));
                                          dto.TestImmutablePrimitiveProperty = "BBB";
                                          testImmutableObjController.Evaluate(c => c.SaveTestImmutableObj(dto.ToStrict()));
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
        var employeeController = this.GetControllerEvaluator<SampleSystem.WebApiCore.Controllers.Main.EmployeeController>();
        var testImmutableObjController = this.GetControllerEvaluator<SampleSystem.WebApiCore.Controllers.Main.TestImmutableObjController>();
        var identity = testImmutableObjController.Evaluate(c => c.SaveTestImmutableObj(new TestImmutableObjStrictDTO { }));

        // Act
        Action changePropertyAction = () =>
                                      {
                                          var dto = testImmutableObjController.Evaluate(c => c.GetRichTestImmutableObj(identity));
                                          dto.TestImmutableRefProperty = this.DataHelper.GetCurrentEmployee();
                                          testImmutableObjController.Evaluate(c => c.SaveTestImmutableObj(dto.ToStrict()));
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
        var integrationController = this.GetControllerEvaluator<SampleSystem.WebApiCore.Controllers.Integration.TestImmutableObjController>().WithImpersonate(DefaultConstants.INTEGRATION_BUS);

        // Act
        Action insertAction = () => integrationController.Evaluate(c => c.SaveTestImmutableObj(new TestImmutableObjIntegrationRichDTO { TestImmutablePrimitiveProperty = "AAA", Id = Guid.NewGuid() }));

        // Assert
        insertAction.Should().NotThrow();
    }

    [TestMethod]
    public void ImmutablePropertyChangedByIntegration_RaisedValidationError()
    {
        // Arrange
        var integrationTestImmutableObjController = this.GetControllerEvaluator<SampleSystem.WebApiCore.Controllers.Integration.TestImmutableObjController>().WithImpersonate(DefaultConstants.INTEGRATION_BUS);

        var identity = integrationTestImmutableObjController.Evaluate(c => c.SaveTestImmutableObj(new TestImmutableObjIntegrationRichDTO { TestImmutablePrimitiveProperty = "AAA", Id = Guid.NewGuid() }));

        // Act
        Action changePropertyAction = () => integrationTestImmutableObjController.Evaluate(c => c.SaveTestImmutableObj(new TestImmutableObjIntegrationRichDTO { TestImmutablePrimitiveProperty = "BBB", Id = identity.Id }));

        // Assert
        changePropertyAction.Should().Throw<Exception>($"{nameof(TestImmutableObj.TestImmutablePrimitiveProperty)} field in {nameof(TestImmutableObj)} can't be changed");
    }

}
