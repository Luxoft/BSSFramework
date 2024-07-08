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

    public static SecurityRule.AndSecurityRule And(
        this SecurityRule.DomainObjectSecurityRule securityRule,
        SecurityRule.DomainObjectSecurityRule otherSecurityRule)
    {
        return new SecurityRule.AndSecurityRule(securityRule, otherSecurityRule);
    }

    public static SecurityRule.OrSecurityRule Or(
        this SecurityRole securityRule,
        SecurityRule.DomainObjectSecurityRule otherSecurityRule)
    {
        return securityRule.ToSecurityRule().Or(otherSecurityRule);
    }

    public static SecurityRule.AndSecurityRule And(
        this SecurityRole securityRule,
        SecurityRule.DomainObjectSecurityRule otherSecurityRule)
    {
        return securityRule.ToSecurityRule().And(otherSecurityRule);
    }

    public static SecurityRule.OrSecurityRule Or(
        this SecurityOperation securityOperation,
        SecurityRule.DomainObjectSecurityRule otherSecurityRule)
    {
        return securityOperation.ToSecurityRule().Or(otherSecurityRule);
    }

    public static SecurityRule.AndSecurityRule And(
        this SecurityOperation securityOperation,
        SecurityRule.DomainObjectSecurityRule otherSecurityRule)
    {
        return securityOperation.ToSecurityRule().And(otherSecurityRule);
    }

    public static SecurityRule.OrSecurityRule Or(
        this IEnumerable<SecurityRole> securityRoles,
        SecurityRule.DomainObjectSecurityRule otherSecurityRule)
    {
        return securityRoles.ToSecurityRule().Or(otherSecurityRule);
    }

    public static SecurityRule.AndSecurityRule And(
        this IEnumerable<SecurityRole> securityRoles,
        SecurityRule.DomainObjectSecurityRule otherSecurityRule)
    {
        return securityRoles.ToSecurityRule().And(otherSecurityRule);
    }
}
