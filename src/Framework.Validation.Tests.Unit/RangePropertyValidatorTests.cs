using FluentAssertions;

using Framework.DomainDriven;

using NUnit.Framework;

using NSubstitute;

namespace Framework.Validation.Tests.Unit;

[TestFixture]
public class RangePropertyValidatorTests
{
    private IPropertyValidationContext<DomainObject, decimal> context;

    [SetUp]
    public void Init()
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

    [Test]
    public void GetValidationResult_DecimalValidatorMaxPositiveValue_Success()
    {
        // Arrange
        var validator = RangePropertyValidatorHelper.Decimal.Create<DomainObject>(AvailableValuesHelper.AvailableValues.DecimalRange);
        this.context.Source.Returns(new DomainObject());
        this.context.Value.Returns(999999999999999M);

        // Act
        var result = validator.GetValidationResult(this.context);

        // Assert
        result.Errors.Should().BeEmpty();
    }

    [Test]
    public void GetValidationResult_DecimalValidatorMaxNegativeValue_Success()
    {
        // Arrange
        var validator = RangePropertyValidatorHelper.Decimal.Create<DomainObject>(AvailableValuesHelper.AvailableValues.DecimalRange);
        this.context.Source.Returns(new DomainObject());
        this.context.Value.Returns(-999999999999999M);

        // Act
        var result = validator.GetValidationResult(this.context);

        // Assert
        result.Errors.Should().BeEmpty();
    }

    [Test]
    public void GetValidationResult_DecimalValidatorMaxPositiveValuePlusOne_Failure()
    {
        // Arrange
        var validator = RangePropertyValidatorHelper.Decimal.Create<DomainObject>(AvailableValuesHelper.AvailableValues.DecimalRange);
        this.context.Source.Returns(new DomainObject());
        this.context.Value.Returns(1000000000000000M);

        // Act
        var result = validator.GetValidationResult(this.context);

        // Assert
        result.Errors[0].Message.Should().Be("decimal has Number value was too overflow for a decimal");
    }

    [Test]
    public void GetValidationResult_DecimalValidatorMaxNegativeValueMinusOne_Failure()
    {
        // Arrange
        var validator = RangePropertyValidatorHelper.Decimal.Create<DomainObject>(AvailableValuesHelper.AvailableValues.DecimalRange);
        this.context.Source.Returns(new DomainObject());
        this.context.Value.Returns(-1000000000000000M);

        // Act
        var result = validator.GetValidationResult(this.context);

        // Assert
        result.Errors[0].Message.Should().Be("decimal has Number value was too overflow for a decimal");
    }
}
