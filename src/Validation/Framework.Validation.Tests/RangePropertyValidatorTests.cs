using Framework.Validation.Map;
using Framework.Validation.Validators.DynamicClass.Available.Base;

using NSubstitute;

namespace Framework.Validation.Tests;

public class RangePropertyValidatorTests
{
    private IPropertyValidationContext<DomainObject, decimal> context = null!;

    private readonly Framework.BLL.Validation.AvailableValues availableValues = BLL.Validation.AvailableValues.Default;

    public RangePropertyValidatorTests()
    {
        this.context = Substitute.For<IPropertyValidationContext<DomainObject, decimal>>();
        var map = Substitute.For<IPropertyValidationMap>();
        var validationMap = Substitute.For<IClassValidationMap>();
        this.context.Map.Returns(map);
        map.ReflectedTypeMap.Returns(validationMap);
        map.PropertyName.Returns("Number");

        var property = typeof(DomainObject).GetProperties()[0];
        map.Property.Returns(property);
        map.PropertyTypeMap.Returns(validationMap);
        validationMap.TypeName.Returns("decimal");
    }

    [Fact]
    public void GetValidationResult_DecimalValidatorMaxPositiveValue_Success()
    {
        // Arrange
        var validator = RangePropertyValidatorHelper.Decimal.Create<DomainObject>(this.availableValues.DecimalRange);
        this.context.Source.Returns(new DomainObject());
        this.context.Value.Returns(999999999999999M);

        // Act
        var result = validator.GetValidationResult(this.context);

        // Assert
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void GetValidationResult_DecimalValidatorMaxNegativeValue_Success()
    {
        // Arrange
        var validator = RangePropertyValidatorHelper.Decimal.Create<DomainObject>(this.availableValues.DecimalRange);
        this.context.Source.Returns(new DomainObject());
        this.context.Value.Returns(-999999999999999M);

        // Act
        var result = validator.GetValidationResult(this.context);

        // Assert
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void GetValidationResult_DecimalValidatorMaxPositiveValuePlusOne_Failure()
    {
        // Arrange
        var validator = RangePropertyValidatorHelper.Decimal.Create<DomainObject>(this.availableValues.DecimalRange);
        this.context.Source.Returns(new DomainObject());
        this.context.Value.Returns(1000000000000000M);

        // Act
        var result = validator.GetValidationResult(this.context);

        // Assert
        Assert.Equal("decimal has Number value was too overflow for a decimal", result.Errors[0].Message);
    }

    [Fact]
    public void GetValidationResult_DecimalValidatorMaxNegativeValueMinusOne_Failure()
    {
        // Arrange
        var validator = RangePropertyValidatorHelper.Decimal.Create<DomainObject>(this.availableValues.DecimalRange);
        this.context.Source.Returns(new DomainObject());
        this.context.Value.Returns(-1000000000000000M);

        // Act
        var result = validator.GetValidationResult(this.context);

        // Assert
        Assert.Equal("decimal has Number value was too overflow for a decimal", result.Errors[0].Message);
    }
}
