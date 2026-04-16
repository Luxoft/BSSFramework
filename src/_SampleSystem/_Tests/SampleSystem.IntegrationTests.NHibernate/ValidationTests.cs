using SampleSystem.IntegrationTests.__Support.TestData;

using Xunit;

using ValidationException = Framework.Validation.ValidationException;

namespace SampleSystem.IntegrationTests;

public class ValidationTests : TestBase
{
    [Fact]
    public void ValidateByDB_ValidationException()
    {
        // Arrange
        this.DataHelper.SaveEmployee("John Doe", "JD");

        // Act
        Action call = () => this.DataHelper.SaveEmployee("John Doe", "JD");

        // Assert
        call.Should().Throw<ValidationException>();
    }

    [Fact]
    public void ValidateByClassAttribute_ValidationException()
    {
        // Arrange

        // Act
        Action call = () => this.DataHelper.SaveEmployee(pin: 1234);

        // Assert
        call.Should().Throw<ValidationException>().WithMessage("Employee Pin could not be set as '1234'");
    }

    [Fact]
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

    [Fact]
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

    [Fact]
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

    [Fact]
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
