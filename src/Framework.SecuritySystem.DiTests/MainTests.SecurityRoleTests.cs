using FluentAssertions;

using Microsoft.Extensions.DependencyInjection;

using Xunit;

namespace Framework.SecuritySystem.DiTests;

public partial class MainTests
{
    [Fact]
    public void AdministratorRole_ShouldNotContains_SystemIntegrationRole()
    {
        // Arrange
        var securityRoleSource = this.rootServiceProvider.GetRequiredService<ISecurityRoleSource>();

        var adminRole = securityRoleSource.GetFullRole(SecurityRole.Administrator);

        // Act

        // Assert
        adminRole.Information.Children.Contains(SecurityRole.SystemIntegration).Should().BeFalse();
    }

    [Fact]
    public void SecurityRoleExpander_ExpandDeepChild_AllRolesExpanded()
    {
        // Arrange
        var expander = this.rootServiceProvider.GetRequiredService<SecurityRoleExpander>();

        // Act
        var expandResult = expander.Expand(ExampleSecurityRole.TestRole3.ToSecurityRule());

        // Assert
        expandResult.SecurityRoles.Should().BeEquivalentTo(
            new[]
            {
                SecurityRole.Administrator,
                ExampleSecurityRole.TestRole,
                ExampleSecurityRole.TestRole2,
                ExampleSecurityRole.TestRole3
            });
    }
}
