using Framework.Core;
using Framework.HierarchicalExpand;

using static Framework.SecuritySystem.SecurityRule;

namespace Framework.SecuritySystem;

public static class SecurityRuleExtensions
{
    public static OperationSecurityRule ToSecurityRule(
        this SecurityOperation securityOperation,
        HierarchicalExpandType? customExpandType = null) =>
        new(securityOperation) { CustomExpandType = customExpandType };

    public static NonExpandedRolesSecurityRule ToSecurityRule(
        this IEnumerable<SecurityRole> securityRoles,
        HierarchicalExpandType? customExpandType = null) =>
        new(
        new DeepEqualsCollection<SecurityRole>(securityRoles.OrderBy(sr => sr.Name))) { CustomExpandType = customExpandType };

    public static NonExpandedRolesSecurityRule ToSecurityRule(
        this SecurityRole securityRole,
        HierarchicalExpandType? customExpandType = null) =>
        new[] { securityRole }.ToSecurityRule(customExpandType);

    public static DomainSecurityRule Or(
        this DomainSecurityRule securityRule,
        DomainSecurityRule otherSecurityRule) =>
        new OrSecurityRule(securityRule, otherSecurityRule);

    public static DomainSecurityRule And(
        this DomainSecurityRule securityRule,
        DomainSecurityRule otherSecurityRule) =>
        new AndSecurityRule(securityRule, otherSecurityRule);

    public static DomainSecurityRule Negate(this DomainSecurityRule securityRule) =>
        new NegateSecurityRule(securityRule);

    public static DomainSecurityRule Except(
        this DomainSecurityRule securityRule,
        DomainSecurityRule otherSecurityRule) =>
        securityRule.And(otherSecurityRule.Negate());

    public static DomainSecurityRule Or(
        this SecurityRole securityRule,
        DomainSecurityRule otherSecurityRule) =>
        securityRule.ToSecurityRule().Or(otherSecurityRule);

    public static DomainSecurityRule And(
        this SecurityRole securityRule,
        DomainSecurityRule otherSecurityRule) =>
        securityRule.ToSecurityRule().And(otherSecurityRule);

    public static DomainSecurityRule Negate(this SecurityRole securityRule) => securityRule.ToSecurityRule().Negate();

    public static DomainSecurityRule Except(
        this SecurityRole securityRule,
        DomainSecurityRule otherSecurityRule) =>
        securityRule.ToSecurityRule().Except(otherSecurityRule);

    public static DomainSecurityRule Or(
        this SecurityOperation securityOperation,
        DomainSecurityRule otherSecurityRule) =>
        securityOperation.ToSecurityRule().Or(otherSecurityRule);

    public static DomainSecurityRule And(
        this SecurityOperation securityOperation,
        DomainSecurityRule otherSecurityRule) =>
        securityOperation.ToSecurityRule().And(otherSecurityRule);

    public static DomainSecurityRule Negate(this SecurityOperation securityOperation) =>
        securityOperation.ToSecurityRule().Negate();

    public static DomainSecurityRule Except(
        this SecurityOperation securityOperation,
        DomainSecurityRule otherSecurityRule) =>
        securityOperation.ToSecurityRule().Except(otherSecurityRule);

    public static DomainSecurityRule Or(
        this IEnumerable<SecurityRole> securityRoles,
        DomainSecurityRule otherSecurityRule) =>
        securityRoles.ToSecurityRule().Or(otherSecurityRule);

    public static DomainSecurityRule And(
        this IEnumerable<SecurityRole> securityRoles,
        DomainSecurityRule otherSecurityRule) =>
        securityRoles.ToSecurityRule().And(otherSecurityRule);

    public static DomainSecurityRule Negate(this IEnumerable<SecurityRole> securityRoles) =>
        securityRoles.ToSecurityRule().Negate();

    public static DomainSecurityRule Except(
        this IEnumerable<SecurityRole> securityRoles,
        DomainSecurityRule otherSecurityRule) =>
        securityRoles.ToSecurityRule().Except(otherSecurityRule);

    public static DomainSecurityRule WithOverrideAccessDeniedMessage(
        this DomainSecurityRule securityRule,
        string customMessage) =>
        new OverrideAccessDeniedMessageSecurityRule(securityRule, customMessage);
}
