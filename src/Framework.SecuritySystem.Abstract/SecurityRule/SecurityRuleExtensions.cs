using Framework.Core;
using Framework.HierarchicalExpand;

namespace Framework.SecuritySystem;

public static class SecurityRuleExtensions
{
    public static SecurityRule.OperationSecurityRule ToSecurityRule(
        this SecurityOperation securityOperation,
        HierarchicalExpandType? customExpandType = null) =>
        new(securityOperation) { CustomExpandType = customExpandType };

    public static SecurityRule.NonExpandedRolesSecurityRule ToSecurityRule(
        this IEnumerable<SecurityRole> securityRoles,
        HierarchicalExpandType? customExpandType = null) =>
        new(
        new DeepEqualsCollection<SecurityRole>(securityRoles.OrderBy(sr => sr.Name))) { CustomExpandType = customExpandType };

    public static SecurityRule.NonExpandedRolesSecurityRule ToSecurityRule(
        this SecurityRole securityRole,
        HierarchicalExpandType? customExpandType = null) =>
        new[] { securityRole }.ToSecurityRule(customExpandType);

    public static SecurityRule.DomainSecurityRule Or(
        this SecurityRule.DomainSecurityRule securityRule,
        SecurityRule.DomainSecurityRule otherSecurityRule) =>
        new SecurityRule.OrSecurityRule(securityRule, otherSecurityRule);

    public static SecurityRule.DomainSecurityRule And(
        this SecurityRule.DomainSecurityRule securityRule,
        SecurityRule.DomainSecurityRule otherSecurityRule) =>
        new SecurityRule.AndSecurityRule(securityRule, otherSecurityRule);

    public static SecurityRule.DomainSecurityRule Negate(this SecurityRule.DomainSecurityRule securityRule) =>
        new SecurityRule.NegateSecurityRule(securityRule);

    public static SecurityRule.DomainSecurityRule Except(
        this SecurityRule.DomainSecurityRule securityRule,
        SecurityRule.DomainSecurityRule otherSecurityRule) =>
        securityRule.And(otherSecurityRule.Negate());

    public static SecurityRule.DomainSecurityRule Or(
        this SecurityRole securityRule,
        SecurityRule.DomainSecurityRule otherSecurityRule) =>
        securityRule.ToSecurityRule().Or(otherSecurityRule);

    public static SecurityRule.DomainSecurityRule And(
        this SecurityRole securityRule,
        SecurityRule.DomainSecurityRule otherSecurityRule) =>
        securityRule.ToSecurityRule().And(otherSecurityRule);

    public static SecurityRule.DomainSecurityRule Negate(this SecurityRole securityRule) => securityRule.ToSecurityRule().Negate();

    public static SecurityRule.DomainSecurityRule Except(
        this SecurityRole securityRule,
        SecurityRule.DomainSecurityRule otherSecurityRule) =>
        securityRule.ToSecurityRule().Except(otherSecurityRule);

    public static SecurityRule.DomainSecurityRule Or(
        this SecurityOperation securityOperation,
        SecurityRule.DomainSecurityRule otherSecurityRule) =>
        securityOperation.ToSecurityRule().Or(otherSecurityRule);

    public static SecurityRule.DomainSecurityRule And(
        this SecurityOperation securityOperation,
        SecurityRule.DomainSecurityRule otherSecurityRule) =>
        securityOperation.ToSecurityRule().And(otherSecurityRule);

    public static SecurityRule.DomainSecurityRule Negate(this SecurityOperation securityOperation) =>
        securityOperation.ToSecurityRule().Negate();

    public static SecurityRule.DomainSecurityRule Except(
        this SecurityOperation securityOperation,
        SecurityRule.DomainSecurityRule otherSecurityRule) =>
        securityOperation.ToSecurityRule().Except(otherSecurityRule);

    public static SecurityRule.DomainSecurityRule Or(
        this IEnumerable<SecurityRole> securityRoles,
        SecurityRule.DomainSecurityRule otherSecurityRule) =>
        securityRoles.ToSecurityRule().Or(otherSecurityRule);

    public static SecurityRule.DomainSecurityRule And(
        this IEnumerable<SecurityRole> securityRoles,
        SecurityRule.DomainSecurityRule otherSecurityRule) =>
        securityRoles.ToSecurityRule().And(otherSecurityRule);

    public static SecurityRule.DomainSecurityRule Negate(this IEnumerable<SecurityRole> securityRoles) =>
        securityRoles.ToSecurityRule().Negate();

    public static SecurityRule.DomainSecurityRule Except(
        this IEnumerable<SecurityRole> securityRoles,
        SecurityRule.DomainSecurityRule otherSecurityRule) =>
        securityRoles.ToSecurityRule().Except(otherSecurityRule);
}
