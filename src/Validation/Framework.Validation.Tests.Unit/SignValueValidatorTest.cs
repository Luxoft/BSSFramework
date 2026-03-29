using FluentAssertions;

using NUnit.Framework;

using NSubstitute;

namespace Framework.Validation.Test;

[TestFixture]
public class SignValueValidatorTest
{
    private const string PropertyName = "p";

    private const string TypeName = "t";

    private IPropertyValidationContext<object, object> context;
    private IPropertyValidationMap map;
    private IClassValidationMap validationMap;

    [SetUp]
    public void Init()
    {
        this.context = Substitute.For<IPropertyValidationContext<object, object>>();
        this.map = Substitute.For<IPropertyValidationMap>();
        this.validationMap = Substitute.For<IClassValidationMap>();
        this.context.Map.Returns(this.map);
        this.map.ReflectedTypeMap.Returns(this.validationMap);
        this.map.PropertyName.Returns(PropertyName);
        this.validationMap.TypeName.Returns(TypeName);
    }

    [TestCase(SignType.Positive)]
    [TestCase(SignType.Zero)]
    [TestCase(SignType.Negative)]
    [TestCase(SignType.ZeroAndPositive)]
    public void GetValidationResult_NullValue_NoErrors(SignType signType)
    {
        // Arrange
        var validator = new SignValidator(signType);
        this.context.Source.Returns(new object());
        this.context.Value.Returns(null);

        // Act
        var result = validator.GetValidationResult(this.context);

        // Assert
        result.Errors.Should().BeEmpty();
    }

    [TestCase(0)]
    [TestCase(1)]
    public void GetValidationResult_ZeroAndPositiveCorrectValue_NoErrors(int value)
    {
        // Arrange
        var validator = new SignValidator(SignType.ZeroAndPositive);
        this.context.Source.Returns(new object());
        this.context.Value.Returns(value);

        // Act
        var result = validator.GetValidationResult(this.context);

        // Assert
        result.Errors.Should().BeEmpty();
    }

    [Test]
    public void GetValidationResult_ZeroAndPositiveIncorrectValue_ErrorMessageInExpectedFormat()
    {
        // Arrange
        var validator = new SignValidator(SignType.ZeroAndPositive);
        this.context.Source.Returns(new object());
        this.context.Value.Returns(-1);

        // Act
        var result = validator.GetValidationResult(this.context);

        // Assert
        result.Errors[0].Message.Should().Be("The p of t must be greater or equal 0");
    }

    [Test]
    public void GetValidationResult_PositiveCorrectValue_NoErrors()
    {
        // Arrange
        var validator = new SignValidator(SignType.Positive);
        this.context.Source.Returns(new object());
        this.context.Value.Returns(1);

        // Act
        var result = validator.GetValidationResult(this.context);

        // Assert
        result.Errors.Should().BeEmpty();
    }

    [Test]
    public void GetValidationResult_PositiveNegativeValue_ErrorMessageInExpectedFormat()
    {
        // Arrange
        var validator = new SignValidator(SignType.Positive);
        this.context.Source.Returns(new object());
        this.context.Value.Returns(-1);

        // Act
        var result = validator.GetValidationResult(this.context);

        // Assert
        result.Errors[0].Message.Should().Be("The p of t must be greater than 0");
    }

    [Test]
    public void GetValidationResult_PositiveZeroValue_ErrorMessageInExpectedFormat()
    {
        // Arrange
        var validator = new SignValidator(SignType.Positive);
        this.context.Source.Returns(new object());
        this.context.Value.Returns(0);

        // Act
        var result = validator.GetValidationResult(this.context);

        // Assert
        result.Errors[0].Message.Should().Be("The p of t can not be equal 0");
    }

    [Test]
    public void GetValidationResult_ZeroCorrectValue_NoErrors()
    {
        // Arrange
        var validator = new SignValidator(SignType.Zero);
        this.context.Source.Returns(new object());
        this.context.Value.Returns(0);

        // Act
        var result = validator.GetValidationResult(this.context);

        // Assert
        result.Errors.Should().BeEmpty();
    }

    [TestCase(-1)]
    [TestCase(1)]
    public void GetValidationResult_NotZeroValue_ErrorMessageInExpectedFormat(int value)
    {
        // Arrange
        var validator = new SignValidator(SignType.Zero);
        this.context.Source.Returns(new object());
        this.context.Value.Returns(-1);

        // Act
        var result = validator.GetValidationResult(this.context);

        // Assert
        result.Errors[0].Message.Should().Be("The p of t must be 0");
    }

    [Test]
    public void GetValidationResult_NegativeCorrectValue_NoErrors()
    {
        // Arrange
        var validator = new SignValidator(SignType.Negative);
        this.context.Source.Returns(new object());
        this.context.Value.Returns(-1);

        // Act
        var result = validator.GetValidationResult(this.context);

        // Assert
        result.Errors.Should().BeEmpty();
    }

    [Test]
    public void GetValidationResult_NegativePositiveValue_ErrorMessageInExpectedFormat()
    {
        // Arrange
        var validator = new SignValidator(SignType.Negative);
        this.context.Source.Returns(new object());
        this.context.Value.Returns(1);

        // Act
        var result = validator.GetValidationResult(this.context);

        // Assert
        result.Errors[0].Message.Should().Be("The p of t must be less than 0");
    }

    [Test]
    public void GetValidationResult_NegativeZeroValue_ErrorMessageInExpectedFormat()
    {
        // Arrange
        var validator = new SignValidator(SignType.Negative);
        this.context.Source.Returns(new object());
        this.context.Value.Returns(0);

        // Act
        var result = validator.GetValidationResult(this.context);

        // Assert
        result.Errors[0].Message.Should().Be("The p of t can not be equal 0");
    }

    [Test]
    public void GetValidationResult_NullContext_ThrowArgumentNullException()
    {
        // Arrange
        var validator = new SignValidator(SignType.Negative);

        // Act
        object TestDelegate() => validator.GetValidationResult(null);

        // Assert
        Assert.That(TestDelegate, Throws.TypeOf<ArgumentNullException>());
    }
}
