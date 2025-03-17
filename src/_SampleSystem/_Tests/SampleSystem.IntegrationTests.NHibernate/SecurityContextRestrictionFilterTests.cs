using SampleSystem.IntegrationTests.__Support.TestData;
using SampleSystem.Security;

namespace SampleSystem.IntegrationTests;

[TestClass]
public class SecurityContextRestrictionFilterTests : TestBase
{
    [TestMethod]
    public void CreatePermissionWithRestrictionFilter_ApplyInvalidBusinessUnit_ExceptionRaised()
    {
        // Arrange
        var employee = this.DataHelper.SaveEmployee();

        var bu = this.DataHelper.SaveBusinessUnit();

        // Act
        var action = () => this.AuthHelper.SetUserRole(
                         employee.Id,
                         new SampleSystemTestPermission(SampleSystemSecurityRole.WithRestrictionFilterRole) { BusinessUnit = bu });

        // Assert
        action.Should().Throw<ValidationException>().And.Message.Should().Contain($"SecurityContext: '{bu.Id}' denied by filter.");
    }

    [TestMethod]
    public void CreatePermissionWithRestrictionFilter_ApplyCorrectBusinessUnit_ExceptionNotRaised()
    {
        // Arrange
        var employee = this.DataHelper.SaveEmployee();

        var bu = this.DataHelper.SaveBusinessUnit(allowedForFilterRole: true);

        // Act
        var action = () => this.AuthHelper.SetUserRole(
                         employee.Id,
                         new SampleSystemTestPermission(SampleSystemSecurityRole.WithRestrictionFilterRole) { BusinessUnit = bu });

        // Assert
        action.Should().NotThrow();
    }
}
