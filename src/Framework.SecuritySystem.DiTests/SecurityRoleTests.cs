using FluentAssertions;

using Xunit;

namespace Framework.SecuritySystem.DiTests;

public class SecurityRoleTests
{
    [Fact]
    public async Task AdministratorRole_ShouldNotContains_SystemIntegrationRole()
    {
        // Arrange
        var adminRole = ExampleSecurityRole.Administrator;

        // Act

        // Assert
        adminRole.Children.Contains(ExampleSecurityRole.SystemIntegration).Should().BeFalse();
    }
}
