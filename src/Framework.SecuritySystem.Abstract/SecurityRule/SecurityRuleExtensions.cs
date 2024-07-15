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

    public static SecurityRule.DomainSecurityRule Or(
        this SecurityRule.DomainSecurityRule securityRule,
        SecurityRule.DomainSecurityRule otherSecurityRule)
    {
        return new SecurityRule.OrSecurityRule(securityRule, otherSecurityRule);
    }

    public static SecurityRule.DomainSecurityRule And(
        this SecurityRule.DomainSecurityRule securityRule,
        SecurityRule.DomainSecurityRule otherSecurityRule)
    {
        return new SecurityRule.AndSecurityRule(securityRule, otherSecurityRule);
    }

    public static SecurityRule.DomainSecurityRule Negate(this SecurityRule.DomainSecurityRule securityRule)
    {
        return new SecurityRule.NegateSecurityRule(securityRule);
    }

    public static SecurityRule.DomainSecurityRule Except(
        this SecurityRule.DomainSecurityRule securityRule,
        SecurityRule.DomainSecurityRule otherSecurityRule)
    {
        return securityRule.And(otherSecurityRule.Negate());
    }

    public static SecurityRule.DomainSecurityRule Or(
        this SecurityRole securityRule,
        SecurityRule.DomainSecurityRule otherSecurityRule)
    {
        return securityRule.ToSecurityRule().Or(otherSecurityRule);
    }

    public static SecurityRule.DomainSecurityRule And(
        this SecurityRole securityRule,
        SecurityRule.DomainSecurityRule otherSecurityRule)
    {
        return securityRule.ToSecurityRule().And(otherSecurityRule);
    }

    public static SecurityRule.DomainSecurityRule Negate(this SecurityRole securityRule)
    {
        return securityRule.ToSecurityRule().Negate();
    }

    public static SecurityRule.DomainSecurityRule Except(
        this SecurityRole securityRule,
        SecurityRule.DomainSecurityRule otherSecurityRule)
    {
        return securityRule.ToSecurityRule().Except(otherSecurityRule);
    }

    public static SecurityRule.DomainSecurityRule Or(
        this SecurityOperation securityOperation,
        SecurityRule.DomainSecurityRule otherSecurityRule)
    {
        return securityOperation.ToSecurityRule().Or(otherSecurityRule);
    }

    public static SecurityRule.DomainSecurityRule And(
        this SecurityOperation securityOperation,
        SecurityRule.DomainSecurityRule otherSecurityRule)
    {
        return securityOperation.ToSecurityRule().And(otherSecurityRule);
    }

    public static SecurityRule.DomainSecurityRule Negate(this SecurityOperation securityOperation)
    {
        return securityOperation.ToSecurityRule().Negate();
    }

    public static SecurityRule.DomainSecurityRule Except(
        this SecurityOperation securityOperation,
        SecurityRule.DomainSecurityRule otherSecurityRule)
    {
        return securityOperation.ToSecurityRule().Except(otherSecurityRule);
    }

    public static SecurityRule.DomainSecurityRule Or(
        this IEnumerable<SecurityRole> securityRoles,
        SecurityRule.DomainSecurityRule otherSecurityRule)
    {
        return securityRoles.ToSecurityRule().Or(otherSecurityRule);
    }

    public static SecurityRule.DomainSecurityRule And(
        this IEnumerable<SecurityRole> securityRoles,
        SecurityRule.DomainSecurityRule otherSecurityRule)
    {
        return securityRoles.ToSecurityRule().And(otherSecurityRule);
    }

    public static SecurityRule.DomainSecurityRule Negate(this IEnumerable<SecurityRole> securityRoles)
    {
        return securityRoles.ToSecurityRule().Negate();
    }

    public static SecurityRule.DomainSecurityRule Except(
        this IEnumerable<SecurityRole> securityRoles,
        SecurityRule.DomainSecurityRule otherSecurityRule)
    {
        return securityRoles.ToSecurityRule().Except(otherSecurityRule);
    }
}
