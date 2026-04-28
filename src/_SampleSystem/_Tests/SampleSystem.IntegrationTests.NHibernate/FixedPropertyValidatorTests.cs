using SampleSystem.Domain;
using SampleSystem.Generated.DTO;
using SampleSystem.IntegrationTests._Environment.TestData;
using SampleSystem.WebApiCore.Controllers.Main;

namespace SampleSystem.IntegrationTests;

public class FixedPropertyValidatorTests(IServiceProvider rootServiceProvider) : TestBase(rootServiceProvider)
{
    [Fact]
    public void PrimitiveImmutablePropertyChanged_RaisedValidationError()
    {
        // Arrange
        var testImmutableObjController = this.GetControllerEvaluator<TestImmutableObjController>();
        var identity = testImmutableObjController.Evaluate(c => c.SaveTestImmutableObj(
                                                                                       new TestImmutableObjStrictDTO { TestImmutablePrimitiveProperty = "AAA" }));

        // Act
        var ex = Record.Exception(() =>
        {
            var dto = testImmutableObjController.Evaluate(c => c.GetRichTestImmutableObj(identity));
            dto.TestImmutablePrimitiveProperty = "BBB";
            testImmutableObjController.Evaluate(c => c.SaveTestImmutableObj(dto.ToStrict()));
        });

        // Assert
        var validationException = Assert.IsType<Framework.Validation.ValidationException>(ex);
        Assert.Equal(
            $"{nameof(TestImmutableObj.TestImmutablePrimitiveProperty)} field in {nameof(TestImmutableObj)} can't be changed",
            validationException.Message);
    }

    [Fact]
    public void ReferenceImmutablePropertyChanged_RaisedValidationError()
    {
        // Arrange
        var employeeController = this.GetControllerEvaluator<EmployeeController>();
        var testImmutableObjController = this.GetControllerEvaluator<TestImmutableObjController>();
        var identity = testImmutableObjController.Evaluate(c => c.SaveTestImmutableObj(new TestImmutableObjStrictDTO { }));

        // Act
        var ex = Record.Exception(() =>
        {
            var dto = testImmutableObjController.Evaluate(c => c.GetRichTestImmutableObj(identity));
            dto.TestImmutableRefProperty = this.DataHelper.GetCurrentEmployee();
            testImmutableObjController.Evaluate(c => c.SaveTestImmutableObj(dto.ToStrict()));
        });

        // Assert
        var validationException = Assert.IsType<Framework.Validation.ValidationException>(ex);
        Assert.Equal(
            $"{nameof(TestImmutableObj.TestImmutableRefProperty)} field in {nameof(TestImmutableObj)} can't be changed",
            validationException.Message);
    }

    [Fact]
    public void ImmutablePropertyInitializedByIntegration_ShouldNotThrowException()
    {
        // Arrange
        var integrationController = this.GetControllerEvaluator<SampleSystem.WebApiCore.Controllers.Integration.TestImmutableObjController>().WithImpersonate(DefaultConstants.INTEGRATION_BUS);

        // Act
        Action insertAction = () => integrationController.Evaluate(c => c.SaveTestImmutableObj(new TestImmutableObjIntegrationRichDTO { TestImmutablePrimitiveProperty = "AAA", Id = Guid.NewGuid() }));

        // Assert
        insertAction();
    }

    [Fact]
    public void ImmutablePropertyChangedByIntegration_RaisedValidationError()
    {
        // Arrange
        var integrationTestImmutableObjController = this.GetControllerEvaluator<SampleSystem.WebApiCore.Controllers.Integration.TestImmutableObjController>().WithImpersonate(DefaultConstants.INTEGRATION_BUS);

        var identity = integrationTestImmutableObjController.Evaluate(c => c.SaveTestImmutableObj(new TestImmutableObjIntegrationRichDTO { TestImmutablePrimitiveProperty = "AAA", Id = Guid.NewGuid() }));

        // Act
        var ex = Record.Exception(() => integrationTestImmutableObjController.Evaluate(c => c.SaveTestImmutableObj(new TestImmutableObjIntegrationRichDTO { TestImmutablePrimitiveProperty = "BBB", Id = identity.Id })));

        // Assert
        var validationException = Assert.IsType<Framework.Validation.ValidationException>(ex);
        Assert.Equal(
            $"{nameof(TestImmutableObj.TestImmutablePrimitiveProperty)} field in {nameof(TestImmutableObj)} can't be changed",
            validationException.Message);
    }

}
