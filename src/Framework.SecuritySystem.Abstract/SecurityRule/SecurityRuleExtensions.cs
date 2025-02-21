using System.Linq.Expressions;

using Framework.Core;
using Framework.HierarchicalExpand;

using static Framework.SecuritySystem.DomainSecurityRule;

namespace Framework.SecuritySystem;

public static class SecurityRuleExtensions
{
    public static TSecurityRule TryApplyCredential<TSecurityRule>(this TSecurityRule securityRule, SecurityRuleCredential credential)
        where TSecurityRule : RoleBaseSecurityRule =>
        securityRule.CustomCredential == null ? securityRule with { CustomCredential = credential } : securityRule;

    public static TSecurityRule WithoutRunAs<TSecurityRule>(this TSecurityRule securityRule)
        where TSecurityRule : RoleBaseSecurityRule =>
        securityRule with { CustomCredential = new SecurityRuleCredential.CurrentUserWithoutRunAsCredential() };

    public static OperationSecurityRule ToSecurityRule(
        this SecurityOperation securityOperation,
        HierarchicalExpandType? customExpandType = null,
        SecurityRuleCredential? customCredential = null,
        SecurityPathRestriction? customRestriction = null) =>
        new(securityOperation)
        {
            CustomExpandType = customExpandType, CustomCredential = customCredential, CustomRestriction = customRestriction
        };

    public static NonExpandedRolesSecurityRule ToSecurityRule(
        this IEnumerable<SecurityRole> securityRoles,
        HierarchicalExpandType? customExpandType = null,
        SecurityRuleCredential? customCredential = null,
        SecurityPathRestriction? customRestriction = null) =>
        new(
        securityRoles.OrderBy(sr => sr.Name).ToArray())
        {
            CustomExpandType = customExpandType, CustomCredential = customCredential, CustomRestriction = customRestriction
        };

    public static NonExpandedRolesSecurityRule ToSecurityRule(
        this SecurityRole securityRole,
        HierarchicalExpandType? customExpandType = null,
        SecurityRuleCredential? customCredential = null,
        SecurityPathRestriction? customRestriction = null) =>
        new[] { securityRole }.ToSecurityRule(customExpandType, customCredential, customRestriction);

    public static RoleBaseSecurityRule ToSecurityRule(
        this IEnumerable<RoleBaseSecurityRule> securityRules,
        HierarchicalExpandType? customExpandType = null,
        SecurityRuleCredential? customCredential = null,
        SecurityPathRestriction? customRestriction = null)
    {
        var cache = securityRules.ToList();

        if (cache.Count == 1)
        {
            return cache.Single().TryApplyCustoms(customExpandType, customCredential, customRestriction);
        }
        else
        {
            return new RoleGroupSecurityRule(
                   cache.ToArray())
                   {
                       CustomExpandType = customExpandType, CustomCredential = customCredential, CustomRestriction = customRestriction
                   };
        }
    }

    public static DomainSecurityRule Or(
        this DomainSecurityRule securityRule,
        DomainSecurityRule otherSecurityRule) =>
        new OrSecurityRule(securityRule, otherSecurityRule);

    public static DomainSecurityRule And(
        this DomainSecurityRule securityRule,
        DomainSecurityRule otherSecurityRule) =>
        new AndSecurityRule(securityRule, otherSecurityRule);

    public static DomainSecurityRule Or<TRelativeDomainObject>(
        this DomainSecurityRule securityRule,
        Expression<Func<TRelativeDomainObject, bool>> condition) =>
        securityRule.Or(new RelativeConditionSecurityRule(condition.ToInfo()));

    public static DomainSecurityRule And<TRelativeDomainObject>(
        this DomainSecurityRule securityRule,
        Expression<Func<TRelativeDomainObject, bool>> condition) =>
        securityRule.And(new RelativeConditionSecurityRule(condition.ToInfo()));

    public static DomainSecurityRule Except<TRelativeDomainObject>(
        this DomainSecurityRule securityRule,
        Expression<Func<TRelativeDomainObject, bool>> condition) =>
        securityRule.And(condition.Not());

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

    public static DomainSecurityRule Or<TRelativeDomainObject>(
        this SecurityRole securityRule,
        Expression<Func<TRelativeDomainObject, bool>> condition) =>
        securityRule.ToSecurityRule().Or(condition);

    public static DomainSecurityRule And<TRelativeDomainObject>(
        this SecurityRole securityRule,
        Expression<Func<TRelativeDomainObject, bool>> condition) =>
        securityRule.ToSecurityRule().And(condition);

    public static DomainSecurityRule Except<TRelativeDomainObject>(
        this SecurityRole securityRule,
        Expression<Func<TRelativeDomainObject, bool>> condition) =>
        securityRule.ToSecurityRule().Except(condition);

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

    public static DomainSecurityRule Or<TRelativeDomainObject>(
        this SecurityOperation securityRule,
        Expression<Func<TRelativeDomainObject, bool>> condition) =>
        securityRule.ToSecurityRule().Or(condition);

    public static DomainSecurityRule And<TRelativeDomainObject>(
        this SecurityOperation securityRule,
        Expression<Func<TRelativeDomainObject, bool>> condition) =>
        securityRule.ToSecurityRule().And(condition);

    public static DomainSecurityRule Except<TRelativeDomainObject>(
        this SecurityOperation securityRule,
        Expression<Func<TRelativeDomainObject, bool>> condition) =>
        securityRule.ToSecurityRule().Except(condition);

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

    public static DomainSecurityRule Or<TRelativeDomainObject>(
        this IEnumerable<SecurityRole> securityRoles,
        Expression<Func<TRelativeDomainObject, bool>> condition) =>
        securityRoles.ToSecurityRule().Or(condition);

    public static DomainSecurityRule And<TRelativeDomainObject>(
        this IEnumerable<SecurityRole> securityRoles,
        Expression<Func<TRelativeDomainObject, bool>> condition) =>
        securityRoles.ToSecurityRule().And(condition);

    public static DomainSecurityRule Except<TRelativeDomainObject>(
        this IEnumerable<SecurityRole> securityRoles,
        Expression<Func<TRelativeDomainObject, bool>> condition) =>
        securityRoles.ToSecurityRule().Except(condition);

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

    public static T TryApplyCustoms<T>(
        this T securityRule,
        HierarchicalExpandType? customExpandType = null,
        SecurityRuleCredential? customCredential = null,
        SecurityPathRestriction? customRestriction = null)
        where T : RoleBaseSecurityRule =>

        customExpandType is null && customCredential is null && customRestriction is null
            ? securityRule
            : securityRule with
              {
                  CustomExpandType = securityRule.CustomExpandType ?? customExpandType,
                  CustomCredential = securityRule.CustomCredential ?? customCredential,
                  CustomRestriction = securityRule.CustomRestriction ?? customRestriction,
              };

    public static T TryApplyCustoms<T>(this T securityRule, IRoleBaseSecurityRuleCustomData customSource)
        where T : RoleBaseSecurityRule =>
        securityRule.TryApplyCustoms(customSource.CustomExpandType, customSource.CustomCredential, customSource.CustomRestriction);
}
