#nullable enable
using Framework.Core;
using Framework.HierarchicalExpand;

namespace Framework.SecuritySystem;

public static class SecurityRuleExtensions
{
    public static SecurityRule.OperationSecurityRule ToSecurityRule(
        this SecurityOperation securityOperation,
        HierarchicalExpandType? customExpandType = null)
    {
        return new SecurityRule.OperationSecurityRule(securityOperation) { CustomExpandType = customExpandType };
    }

    public static SecurityRule.NonExpandedRolesSecurityRule ToSecurityRule(
        this IEnumerable<SecurityRole> securityRoles,
        HierarchicalExpandType? customExpandType = null)
    {
        return new SecurityRule.NonExpandedRolesSecurityRule(
               new DeepEqualsCollection<SecurityRole>(securityRoles.OrderBy(sr => sr.Name))) { CustomExpandType = customExpandType };
    }

    public static SecurityRule.NonExpandedRolesSecurityRule ToSecurityRule(
        this SecurityRole securityRole,
        HierarchicalExpandType? customExpandType = null)
    {
        return new[] { securityRole }.ToSecurityRule(customExpandType);
    }

    public static SecurityRule.OrSecurityRule Or(
        this SecurityRule.DomainObjectSecurityRule securityRule,
        SecurityRule.DomainObjectSecurityRule otherSecurityRule)
    {
        return new SecurityRule.OrSecurityRule(securityRule, otherSecurityRule);
    }

    public static SecurityRule.OrSecurityRule OrCustomProvider(
        this SecurityRule.DomainObjectSecurityRule securityRule,
        Type genericCustomProviderType)
    {
        return securityRule.Or(new SecurityRule.CustomProviderSecurityRule(genericCustomProviderType));
    }

    public static SecurityRule.AndSecurityRule And(
        this SecurityRule.DomainObjectSecurityRule securityRule,
        SecurityRule.DomainObjectSecurityRule otherSecurityRule)
    {
        return new SecurityRule.AndSecurityRule(securityRule, otherSecurityRule);
    }

    public static SecurityRule.AndSecurityRule AndCustomProvider(
        this SecurityRule.DomainObjectSecurityRule securityRule,
        Type genericCustomProviderType)
    {
        return securityRule.And(new SecurityRule.CustomProviderSecurityRule(genericCustomProviderType));
    }
}
