using FluentAssertions;
using Framework.HierarchicalExpand;
using Framework.SecuritySystem.Expanders;

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

        // Act
        var adminRole = securityRoleSource.GetSecurityRole(SecurityRole.Administrator);

        // Assert
        adminRole.Information.Children.Contains(SecurityRole.SystemIntegration).Should().BeFalse();
    }

    [Fact]
    public void SecurityRoleExpander_ExpandDeepChild_AllRolesExpanded()
    {
        // Arrange
        var expander = this.rootServiceProvider.GetRequiredService<ISecurityRoleExpander>();

        // Act
        var expandResult = expander.Expand(ExampleSecurityRole.TestRole3);

        // Assert
        expandResult.SecurityRoles.Should().BeEquivalentTo(
            new[]
            {
                SecurityRole.Administrator, ExampleSecurityRole.TestRole, ExampleSecurityRole.TestRole2, ExampleSecurityRole.TestRole3
            });
    }

    [Fact]
    public void SecurityRoleExpander_ExpandWithDefaultExpandType_RoleResolved()
    {
        // Arrange
        var expander = this.rootServiceProvider.GetRequiredService<ISecurityOperationExpander>();

        // Act
        var expandResult = expander.Expand(new DomainSecurityRule.OperationSecurityRule(ExampleSecurityOperation.EmployeeView));

        // Assert
        expandResult.SecurityRoles.Should().BeEquivalentTo(new[] { ExampleSecurityRole.TestRole });
    }

    [Fact]
    public void SecurityRoleExpander_ExpandWithCustomExpandType_SecurityRuleCorrected()
    {
        // Arrange
        var expander = this.rootServiceProvider.GetRequiredService<ISecurityOperationExpander>();

        // Act
        var expandResult = expander.Expand(ExampleSecurityOperation.EmployeeView.ToSecurityRule(HierarchicalExpandType.None));

        // Assert
        expandResult.Should().BeEquivalentTo(new[] { ExampleSecurityRole.TestRole }.ToSecurityRule(HierarchicalExpandType.None));
    }

    [Fact]
    public void SecurityRoleExpander_FullExpandWithCustomExpandType_SecurityRuleCorrected()
    {
        // Arrange
        var expander = this.rootServiceProvider.GetRequiredService<ISecurityRuleExpander>();

        // Act
        var expandResult = expander.FullExpand(ExampleSecurityOperation.EmployeeView.ToSecurityRule(HierarchicalExpandType.All));

        // Assert
        expandResult.Should()
                    .Subject
                    .Should()
                    .BeEquivalentTo(
                        new[] { SecurityRole.Administrator, ExampleSecurityRole.TestRole }.ToSecurityRule(HierarchicalExpandType.All));
    }

    [Fact]
    public void SecurityRoleExpander_FullExpandWithCustomExpandTypeFromOperations_SecurityRuleCorrected()
    {
        // Arrange
        var expander = this.rootServiceProvider.GetRequiredService<ISecurityRuleExpander>();

        // Act
        var expandResult = expander.FullExpand(ExampleSecurityOperation.BusinessUnitView);

        // Assert
        expandResult.Should()
                    .Subject
                    .Should()
                    .BeEquivalentTo(
                        new[] { SecurityRole.Administrator, ExampleSecurityRole.TestRole4 }.ToSecurityRule(HierarchicalExpandType.None));
    }
}
