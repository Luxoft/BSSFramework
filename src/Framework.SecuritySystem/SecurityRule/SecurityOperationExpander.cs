#nullable enable

using Framework.Core;

namespace Framework.SecuritySystem;

public class SecurityOperationExpander : ISecurityRuleExpander
{
    private readonly IDictionaryCache<SecurityRule, SecurityRule?> tryExpandCache;

    private readonly IDictionaryCache<SecurityRule.OperationSecurityRule, SecurityRule.RolesSecurityRule> expandCache;

    public SecurityOperationExpander(ISecurityRoleSource securityRoleSource)
    {
        this.tryExpandCache = new DictionaryCache<SecurityRule, SecurityRule?>(
            securityRule =>
            {
                if (securityRule is SecurityRule.OperationSecurityRule operationRule)
                {
                    return this.Expand(operationRule);
                }
                else
                {
                    return null;
                }
            }).WithLock();

        this.expandCache = new DictionaryCache<SecurityRule.OperationSecurityRule, SecurityRule.RolesSecurityRule>(
            securityRule =>
            {
                var securityRoles = securityRoleSource.SecurityRoles.GetAllElements(sr => sr.Children)
                                        .Where(sr => sr.Operations.Contains(securityRule.SecurityOperation))
                                        .Distinct()
                                        .ToArray();

                if (securityRoles.Length == 0)
                {
                    throw new Exception("The list of security roles cannot be empty!");
                }

                return securityRoles.ToSecurityRule();
            }).WithLock();
    }

    public SecurityRule.RolesSecurityRule Expand(SecurityRule.DomainObjectSecurityRule securityRule)
    {
        return securityRule switch
        {
            SecurityRule.RolesSecurityRule rolesSecurityRule => rolesSecurityRule,
            SecurityRule.OperationSecurityRule operationSecurityRule => this.Expand(operationSecurityRule),
            _ => throw new ArgumentOutOfRangeException(nameof(securityRule))
        };
    }

    public SecurityRule.RolesSecurityRule Expand(SecurityRule.OperationSecurityRule securityRule)
    {
        return this.expandCache[securityRule];
    }

    public SecurityRule? TryExpand<TDomainObject>(SecurityRule securityRule)
    {
        return this.tryExpandCache[securityRule];
    }
}
