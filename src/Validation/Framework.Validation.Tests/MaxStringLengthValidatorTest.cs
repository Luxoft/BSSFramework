using Framework.Validation.Map;
using Framework.Validation.Validators;

using NSubstitute;

namespace Framework.Validation.Tests;

public class MaxStringLengthValidatorTest
{
    private const string PropertyName = "p";

    private const string TypeName = "t";

    private const int MaxLength = 5;

    private IPropertyValidationContext<DomainObject, string> context;
    private IPropertyValidationMap map;
    private IClassValidationMap validationMap;

    public MaxStringLengthValidatorTest()
    {
        this.context = Substitute.For<IPropertyValidationContext<DomainObject, string>>();
        this.map = Substitute.For<IPropertyValidationMap>();
        this.validationMap = Substitute.For<IClassValidationMap>();
        this.context.Map.Returns(this.map);
        this.map.ReflectedTypeMap.Returns(this.validationMap);
        this.map.PropertyName.Returns(PropertyName);
        this.validationMap.TypeName.Returns(TypeName);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("a")]
    [InlineData("abcde")]
    public void GetValidationResult_StringValidator_ValueNullOrShorterThanMax_NoErrors(string value)
    {
        // Arrange
        var validator = new MaxLengthValidator.StringMaxLengthValidator<DomainObject>(MaxLength);
        this.context.Source.Returns(new DomainObject());
        this.context.Value.Returns(value);

        // Act
        var result = validator.GetValidationResult(this.context);

        // Assert
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void GetValidationResult_ValueLongerThanMax_ErrorMessageInExpectedFormat()
    {
        // Arrange
        var value = new string('a', MaxLength + 1);
        var validator = new MaxLengthValidator.StringMaxLengthValidator<DomainObject>(MaxLength);
        this.context.Source.Returns(new DomainObject());
        this.context.Value.Returns(value);

        // Act
        var result = validator.GetValidationResult(this.context);

        // Assert
        Assert.Equal($"The length of {PropertyName} property of {TypeName} should not be more than {MaxLength}", result.Errors[0].Message);
    }

    [Fact]
    public void GetValidationResult_NullContext_ThrowArgumentNullException()
    {
        // Arrange
        var validator = new MaxLengthValidator.StringMaxLengthValidator<DomainObject>(MaxLength);

        // Act
        object TestDelegate() => validator.GetValidationResult(null!);

        // Assert
        Assert.Throws<ArgumentNullException>(TestDelegate);
    }
}
