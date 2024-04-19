using FluentAssertions;

using Xunit;

namespace Framework.SecuritySystem.DiTests;

public class SecurityRoleTests
{
    [Fact]
    public void AdministratorRole_ShouldNotContains_SystemIntegrationRole()
    {
        // Arrange
        var adminRole = ExampleSecurityRole.Administrator;

        // Act

        // Assert
        adminRole.Children.Contains(ExampleSecurityRole.SystemIntegration).Should().BeFalse();
    }

    [Fact]
    public void SecurityRoleExpander_ExpandDeepChild_AllRolesExpanded()
    {
        // Arrange
        var expander = new SecurityRoleExpander(new SecurityRoleSource([new SecurityRoleTypeInfo(typeof(ExampleSecurityRole))]));

        // Act

        var expandResult = expander.Expand(ExampleSecurityRole.TestRole3.ToSecurityRule());

        // Assert
        expandResult.SecurityRoles.Should().BeEquivalentTo(
            new[]
            {
                ExampleSecurityRole.Administrator,
                ExampleSecurityRole.TestRole,
                ExampleSecurityRole.TestRole2,
                ExampleSecurityRole.TestRole3
            });
    }
}
