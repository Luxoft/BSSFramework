using System;

using FluentAssertions;
using Framework.Validation.Tests.Unit;

using NSubstitute;

using NUnit.Framework;

namespace Framework.Validation.Test
{
    [TestFixture]
    public class MaxStringLengthValidatorTest
    {
        private const string PropertyName = "p";

        private const string TypeName = "t";

        private const int MaxLength = 5;

        private IPropertyValidationContext<DomainObject, string> context;
        private IPropertyValidationMap map;
        private IClassValidationMap validationMap;

        [SetUp]
        public void Init()
        {
            this.context = Substitute.For<IPropertyValidationContext<DomainObject, string>>();
            this.map = Substitute.For<IPropertyValidationMap>();
            this.validationMap = Substitute.For<IClassValidationMap>();
            this.context.Map.Returns(this.map);
            this.map.ReflectedTypeMap.Returns(this.validationMap);
            this.map.PropertyName.Returns(PropertyName);
            this.validationMap.TypeName.Returns(TypeName);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("a")]
        [TestCase("abcde")]
        public void GetValidationResult_StringValidator_ValueNullOrShorterThanMax_NoErrors(string value)
        {
            // Arrange
            var validator = new MaxLengthValidator.StringMaxLengthValidator<DomainObject>(MaxLength);
            this.context.Source.Returns(new DomainObject());
            this.context.Value.Returns(value);

            // Act
            var result = validator.GetValidationResult(this.context);

            // Assert
            result.Errors.Should().BeEmpty();
        }

        [Test]
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
            result.Errors[0].Message.Should().Be($"The length of {PropertyName} property of {TypeName} should not be more than {MaxLength}");
        }

        [Test]
        public void GetValidationResult_NullContext_ThrowArgumentNullException()
        {
            // Arrange
            var validator = new MaxLengthValidator.StringMaxLengthValidator<DomainObject>(MaxLength);

            // Act
            object TestDelegate() => validator.GetValidationResult(null);

            // Assert
            Assert.That(TestDelegate, Throws.TypeOf<ArgumentNullException>());
        }
    }
}
