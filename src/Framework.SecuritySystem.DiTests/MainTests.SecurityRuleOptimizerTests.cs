using FluentAssertions;

using Framework.Core;
using Framework.SecuritySystem.Services;

using Microsoft.Extensions.DependencyInjection;

using Xunit;

namespace Framework.SecuritySystem.DiTests;

public partial class MainTests
{
    [Theory]
    [MemberData(nameof(OptimizeSecurityRule_RuleOptimized_Data))]
    public void OptimizeSecurityRule_RuleOptimized(SecurityRule.DomainSecurityRule securityRule, SecurityRule.DomainSecurityRule expectedOptimizedSecurityRule)
    {
        //Arrange
        var service = this.rootServiceProvider.GetRequiredService<ISecurityRuleOptimizer>();

        //Act
        var optimizedSecurityPath = service.Optimize(securityRule);

        //Assert
        optimizedSecurityPath.Should().Be(expectedOptimizedSecurityRule);
    }

    public static IEnumerable<object[]> OptimizeSecurityRule_RuleOptimized_Data() =>
        GetCases().Select(pair => new[] { pair.Item1, pair.Item2 });

    private static IEnumerable<(SecurityRule.DomainSecurityRule, SecurityRule.DomainSecurityRule)> GetCases()
    {
        yield return (SecurityRule.CurrentUser.And(SecurityRule.Disabled), SecurityRule.CurrentUser);
        yield return (SecurityRule.Disabled.And(SecurityRule.CurrentUser), SecurityRule.CurrentUser);

        yield return (SecurityRule.CurrentUser.Or(SecurityRule.Disabled), SecurityRule.Disabled);

        yield return (SecurityRole.Administrator.Or(SecurityRole.Administrator), SecurityRole.Administrator);

        yield return
            (
                SecurityRole.Administrator.Or(SecurityRole.SystemIntegration),
                new[] { SecurityRole.Administrator, SecurityRole.SystemIntegration }
            );

        yield return
            (
                SecurityRole.Administrator.Or(SecurityRule.CurrentUser.Or(SecurityRole.SystemIntegration)),
                new[] { SecurityRole.Administrator, SecurityRole.SystemIntegration }.Or(SecurityRule.CurrentUser)
            );
        yield return
            (
                SecurityRole.Administrator.Or(SecurityRole.SystemIntegration.Or(SecurityRule.CurrentUser)),
                new[] { SecurityRole.Administrator, SecurityRole.SystemIntegration }.Or(SecurityRule.CurrentUser)
            );

        yield return
            (
                SecurityRole.Administrator.Or(SecurityRule.CurrentUser).Or(SecurityRole.SystemIntegration),
                SecurityRule.CurrentUser.Or(new[] { SecurityRole.Administrator, SecurityRole.SystemIntegration })
            );

        yield return
            (
                SecurityRule.CurrentUser.Or(SecurityRole.Administrator).Or(SecurityRole.SystemIntegration),
                SecurityRule.CurrentUser.Or(new[] { SecurityRole.Administrator, SecurityRole.SystemIntegration })
            );

        yield return
            (
                SecurityRule.CurrentUser.Negate().Negate(),
                SecurityRule.CurrentUser
            );
    }
}
