using System;

using FluentAssertions;

using Framework.Validation;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleSystem.IntegrationTests.__Support.TestData;

namespace SampleSystem.IntegrationTests
{
    [TestClass]
    public class ValidationTests : TestBase
    {
        [TestMethod]
        [Description("Проверяется уникальности путём запрос к БД.")]
        public void ValidateByDB_ValidationException()
        {
            // Arrange
            this.DataHelper.SaveEmployee("John Doe", "JD");

            // Act
            Action call = () => this.DataHelper.SaveEmployee("John Doe", "JD");

            // Assert
            call.Should().Throw<ValidationException>();
        }

        [Description("Проверка с использованием атрибута.")]
        [TestMethod]
        public void ValidateByClassAttribute_ValidationException()
        {
            // Arrange

            // Act
            Action call = () => this.DataHelper.SaveEmployee(pin: 1234);

            // Assert
            call.Should().Throw<ValidationException>().WithMessage("Employee Pin could not be set as '1234'");
        }

        [TestMethod]
        [Description("Проверка с использованием ValidationMap и валидатора конкретного доменного типа.")]
        public void ValidateByConcreteValidator_ValidationException()
        {
            // Arrange
            var externalId = new Random().Next();
            this.DataHelper.SaveEmployee(externalId: externalId);

            // Act
            Action call = () => this.DataHelper.SaveEmployee(externalId: externalId);

            // Assert
            call.Should().Throw<ValidationException>().WithMessage($"Employee with ExternalId '{externalId}' already exists.");
        }

        [Description("Проверка с использованием глобального валидатора целевой системы.")]
        [TestMethod]
        public void ValidateByGlobalValidator_ValidationException()
        {
            // Arrange
            var pin = new Random().Next();
            this.DataHelper.SaveEmployee(pin: pin);

            // Act
            Action call = () => this.DataHelper.SaveEmployee(pin: pin);

            // Assert
            call.Should().Throw<ValidationException>().WithMessage($"Employee with Pin '{pin}' already exists.");
        }

        [TestMethod]
        [Description("#IADFRAME-334")]
        public void Validate_HasInvalidVirtualProperty_ShouldNotThrow()
        {
            // Arrange
            var externalId = new Random().Next();
            var invalidDate = DateTime.MinValue;

            // Act
            Action call = () => this.DataHelper.SaveEmployee(externalId: externalId, nonValidateVirtualProp: invalidDate);

            // Assert
            call.Should().NotThrow(because: "Non persistent properties must not validated by default");
        }

        [TestMethod]
        [Description("#IADFRAME-334")]
        public void Validate_HasInvalidVirtualPropertyMarkedWithAttribute_ShouldThrow()
        {
            // Arrange
            var externalId = new Random().Next();
            var invalidDate = DateTime.MinValue;

            // Act
            Action call = () => this.DataHelper.SaveEmployee(externalId: externalId, validateVirtualProp: invalidDate);

            // Assert
            call.Should().Throw<ValidationException>().WithMessage("Employee has ValidateVirtualProp value was too overflow for a DateTime");
        }
    }
}
