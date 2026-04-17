using SampleSystem.IntegrationTests.__Support.TestData;

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
        Assert.Throws<ValidationException>(call);
    }

    [Fact]
    public void ValidateByClassAttribute_ValidationException()
    {
        // Arrange

        // Act
        Action call = () => this.DataHelper.SaveEmployee(pin: 1234);

        // Assert
        Assert.Equal("Employee Pin could not be set as '1234'", Assert.Throws<ValidationException>(call).Message);
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
        Assert.Equal($"Employee with ExternalId '{externalId}' already exists.", Assert.Throws<ValidationException>(call).Message);
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
        Assert.Equal($"Employee with Pin '{pin}' already exists.", Assert.Throws<ValidationException>(call).Message);
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
        call();
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
        Assert.Equal("Employee has ValidateVirtualProp value was too overflow for a DateTime", Assert.Throws<ValidationException>(call).Message);
    }
}
