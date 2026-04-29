using SampleSystem.IntegrationTests._Environment.TestData;

using ValidationException = Framework.Validation.ValidationException;

namespace SampleSystem.IntegrationTests;

public class ValidationTests(IServiceProvider rootServiceProvider) : TestBase(rootServiceProvider)
{
    [Fact]
    public void ValidateByDB_ValidationException()
    {
        // Arrange
        this.DataManager.SaveEmployee("John Doe", "JD");

        // Act
        var ex = Record.Exception(() => this.DataManager.SaveEmployee("John Doe", "JD"));

        // Assert
        Assert.IsType<ValidationException>(ex);
    }

    [Fact]
    public void ValidateByClassAttribute_ValidationException()
    {
        // Arrange

        // Act
        var ex = Record.Exception(() => this.DataManager.SaveEmployee(pin: 1234));

        // Assert
        var validationException = Assert.IsType<ValidationException>(ex);
        Assert.Equal("Employee Pin could not be set as '1234'", validationException.Message);
    }

    [Fact]
    public void ValidateByConcreteValidator_ValidationException()
    {
        // Arrange
        var externalId = new Random().Next();
        this.DataManager.SaveEmployee(externalId: externalId);

        // Act
        var ex = Record.Exception(() => this.DataManager.SaveEmployee(externalId: externalId));

        // Assert
        var validationException = Assert.IsType<ValidationException>(ex);
        Assert.Equal($"Employee with ExternalId '{externalId}' already exists.", validationException.Message);
    }

    [Fact]
    public void ValidateByGlobalValidator_ValidationException()
    {
        // Arrange
        var pin = new Random().Next();
        this.DataManager.SaveEmployee(pin: pin);

        // Act
        var ex = Record.Exception(() => this.DataManager.SaveEmployee(pin: pin));

        // Assert
        var validationException = Assert.IsType<ValidationException>(ex);
        Assert.Equal($"Employee with Pin '{pin}' already exists.", validationException.Message);
    }

    [Fact]
    public void Validate_HasInvalidVirtualProperty_ShouldNotThrow()
    {
        // Arrange
        var externalId = new Random().Next();
        var invalidDate = DateTime.MinValue;

        // Act
        Action call = () => this.DataManager.SaveEmployee(externalId: externalId, nonValidateVirtualProp: invalidDate);

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
        var ex = Record.Exception(() => this.DataManager.SaveEmployee(externalId: externalId, validateVirtualProp: invalidDate));

        // Assert
        var validationException = Assert.IsType<ValidationException>(ex);
        Assert.Equal("Employee has ValidateVirtualProp value was too overflow for a DateTime", validationException.Message);
    }
}
